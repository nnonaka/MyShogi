﻿using System.Threading;
using System.Collections.Generic;
using MyShogi.Model.Shogi.Core;
using MyShogi.Model.Shogi.Kifu;
using MyShogi.Model.Shogi.Player;
using MyShogi.Model.Common.ObjectModel;
using System;

namespace MyShogi.Model.LocalServer
{
    /// <summary>
    /// 対局を管理するクラス
    /// 
    /// 内部に局面(Position)を持つ
    /// 内部に棋譜管理クラス(KifuManager)を持つ
    /// 
    /// 思考エンジンへの参照を持つ
    /// プレイヤー入力へのインターフェースを持つ
    /// 時間を管理している。
    /// 
    /// </summary>ない)
    public class LocalGameServer : NotifyObject
    {
        public LocalGameServer()
        {
            kifuManager = new KifuManager();
            Position = kifuManager.Position.Clone(); // immutableでなければならないので、Clone()してセットしておく。

            // 起動時に平手の初期局面が表示されるようにしておく。
            kifuManager.Init();

            KifuList = new List<string>();

            var usiEngine = new UsiEnginePlayer();

            Players = new Player[2];

#if true
            //GameStart(new HumanPlayer(), new HumanPlayer());

            // デバッグ中 後手をUsiEnginePlayerにしてみる。
            GameStart(new HumanPlayer(), usiEngine);
#endif

            // 対局監視スレッドを起動して回しておく。
            var thread = new Thread(thread_worker);
            thread.Start();
        }

        public void Dispose()
        {
            // スレッドを停止させる。
            workerStop = true;
        }

        // -- public properties

        /// <summary>
        /// 局面。これはimmutableなので、メインウインドウの対局画面にdata bindする。
        /// </summary>
        public Position Position
        {
            get { return GetValue<Position>("Position"); }
            set { SetValue<Position>("Position", value); }
        }

        /// <summary>
        /// 棋譜。これをメインウィンドウの棋譜ウィンドウとdata bindする。
        /// </summary>
        public List<string> KifuList
        {
            get { return GetValue<List<string>>("KifuList"); }
            set { SetValue<List<string>>("KifuList", value); }
        }

        /// <summary>
        /// 対局棋譜管理クラス
        /// </summary>
        public KifuManager kifuManager { get; private set; }

        /// <summary>
        /// ユーザーがUI上で操作できるのか？
        /// </summary>
        public bool CanUserMove { get; set; }

        // 仮想プロパティ。Turnが変化した時に"TurnChanged"ハンドラが呼び出される。
        //public bool TurnChanged { }

        /// <summary>
        /// 対局しているプレイヤー
        /// </summary>
        public Player[] Players;

        /// <summary>
        /// c側のプレイヤー
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public Player Player(Color c)
        {
            return Players[(int)c];
        }

        // -- public methods

        /// <summary>
        /// 対局スタート
        /// </summary>
        /// <param name="player1">先手プレイヤー(駒落ちのときは下手)</param>
        /// <param name="player2">後手プレイヤー(駒落ちのときは上手)</param>
        public void GameStart(Player player1 , Player player2 /* 引数、あとで考える */)
        {
            Players[0] = player1;
            Players[1] = player2;

            KifuList = new List<string>(kifuManager.KifuList);

            NotifyTurnChanged();
        }

        /// <summary>
        /// ユーザーから指し手が指された
        /// </summary>
        /// <param name="m"></param>
        public void DoMoveFromUI(Move m)
        {
            var stm = Position.sideToMove;
            var stmPlayer = Players[(int)stm];

            if (stmPlayer.PlayerType == PlayerTypeEnum.Human)
            {
                // これを積んでおけばworker_threadのほうでいずれ処理される。
                stmPlayer.BestMove = m;
            }

        }

        // -- private members

        /// <summary>
        /// スレッドの終了フラグ。
        /// 終了するときにtrueになる。
        /// </summary>
        private bool workerStop = false;

        /// <summary>
        /// 対局中であるかを示すフラグ。
        /// これがfalseであれば対局中ではないので自由に駒を動かせる。
        /// </summary>
        private bool inTheGame = true;

        /// <summary>
        /// スレッドによって実行されていて、対局を管理している。
        /// pooling用のthread。少しダサいが、通知によるコールバックモデルでやるよりコードがシンプルになる。
        /// どのみち持ち時間の監視などを行わないといけないので、このようなworker threadは必要だと思う。
        /// </summary>
        private void thread_worker()
        {
            while (!workerStop)
            {
                foreach (var player in Players)
                {
                    player.OnIdle();
                }

                // 指し手が指されたかのチェック
                CheckMove();

                // 10msごとに各種処理を行う。
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// 指し手が指されたかのチェックを行う
        /// </summary>
        private void CheckMove()
        {
            // 現状の局面の手番側
            var stm = Position.sideToMove;
            var stmPlayer = Player(stm);
            var bestMove = stmPlayer.BestMove; // 指し手
            if (bestMove != Move.NONE)
            {
                stmPlayer.BestMove = Move.NONE; // クリア

                // 駒が動かせる状況でかつ合法手であるなら、受理する。
                if (inTheGame && Position.IsLegal(bestMove))
                {
                    // -- bestMoveを受理して、局面を更新する。

                    var thinkingTime = new TimeSpan(0, 0, 1);
                    kifuManager.Tree.AddNode(bestMove, thinkingTime);
                    kifuManager.Tree.DoMove(bestMove);

                    // -- このクラスのpropertyのPositionを更新する

                    // immutableにするためにClone()してからセットする。
                    // これを更新すると自動的にViewに通知される。

                    Position = kifuManager.Position.Clone();

                    // -- 棋譜が1行追加になったはずなので、それを反映させる。

                    // immutableにするためにClone()してセットしてやり、
                    // 末尾にのみ更新があったことをViewに通知する。

                    var kifuList = new List<string>(kifuManager.KifuList);
                    SetValue("KifuList", kifuList, kifuList.Count - 1); // 末尾が変更になったことを通知
                }

                // -- 次のPlayerに、自分のturnであることを通知してやる。

                NotifyTurnChanged();
            }
        }

        /// <summary>
        /// 手番側のプレイヤーに自分の手番であることを通知するためにThink()を呼び出す。また、CanMove = trueにする。
        /// 非手番側のプレイヤーに対してCanMove = falseにする。
        /// </summary>
        public void NotifyTurnChanged()
        {
            var stm = Position.sideToMove;
            var stmPlayer = Player(stm);

            // USIエンジンのときだけ、"position"コマンドに渡す形で局面図が必要であるから、
            // 生成して、それをPlayer.Think()の引数として渡してやる。
            var isUsiEngine = stmPlayer.PlayerType == PlayerTypeEnum.UsiEngine;
            string usiPosition = isUsiEngine ? kifuManager.UsiPositionString : null;

            stmPlayer.BestMove = stmPlayer.PonderMove = Move.NONE;
            stmPlayer.CanMove = true;
            stmPlayer.Think(usiPosition);

            // 非手番側のCaMoveをfalseに

            var nextPlayer = Player(stm.Not());
            nextPlayer.CanMove = false;

            // CanUserMoveを更新しておいてやる。

            // 値が変わっていなくとも変更通知を送りたいので自力でハンドラを呼び出す。
            CanUserMove = stmPlayer.PlayerType == PlayerTypeEnum.Human && stmPlayer.CanMove;
            RaisePropertyChanged("TurnChanged", CanUserMove); // 仮想プロパティ"TurnChanged"
        }

    }
}