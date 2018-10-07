﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MyShogi.Model.Common.ObjectModel;
using MyShogi.Model.Resource.Images;
using MyShogi.Model.Shogi.Core;

namespace MyShogi.View.Win2D.Setting
{
    public partial class RichSelector : UserControl
    {
        /// <summary>
        /// GroupBox内に複数個のRaidoButtonがあり、そこから選択するためのUserControl。
        /// 画像から選択できる。
        /// </summary>
        public RichSelector()
        {
            InitializeComponent();

            // この2つのControlは位置決めのために配置しているだけ。非表示にして使う。
            radioButton1.Visible = false;
            pictureBox1.Visible = false;

            InitViewModel();

            Disposed += OnDispose;
        }

        public class RichSelectorViewModel : NotifyObject
        {
            /// <summary>
            /// 選択されているRadioButtonの番号。
            /// 変更されるとNotifyObjectなのでイベント通知が飛ぶので、それを捕捉するなりすれば良い。
            /// </summary>
            public int Selection
            {
                get { return GetValue<int>("Selection"); }
                set { SetValue<int>("Selection", value); }
            }
        }
        public RichSelectorViewModel ViewModel = new RichSelectorViewModel();

        private void InitViewModel()
        {
            ViewModel.AddPropertyChangedHandler("Selection", (args) =>
            {
                // 選択されているラジオボタンの番号
                var n = (int)args.value;
                if (radioButtons == null || n < 0 || radioButtons.Length <= n)
                    return;
                var r = radioButtons[n] as RadioButton;
                if (r != null)
                    r.Checked = true;
            });
        }

        /// <summary>
        /// ViewModel.Selectionと、特定のNotifyObjectのnameをOneWayでBindする。
        /// </summary>
        /// <param name="notify"></param>
        /// <param name="name"></param>
        public void Bind(NotifyObject notify , string name)
        {
            ViewModel.AddPropertyChangedHandler("Selection", (args) => { notify.SetValueAndRaisePropertyChanged(name, (int)args.value); });
            ViewModel.Selection = notify.GetValue<int>(name); // いま即座に値を反映させておく。
        }

        #region コントロールとしてのProperty

        /// <summary>
        /// GroupBoxの上に表示するテキスト
        /// </summary>
        public string GroupBoxTitle
        {
            get { return groupBox1.Text; }
            set { groupBox1.Text = value; }
        }

        /// <summary>
        /// それぞれの選択肢のテキスト
        /// この数だけRadioButtonを生成する。
        ///
        /// "表示する,rank_0.png"のように表示名と画像ファイル名を書く。
        /// </summary>
        public string [] SelectionTexts { get; set; }

        #endregion

        #region privates

        private void ResizeRadioButtons(int n)
        {
            if (radioButtons != null && radioButtons.Length == n)
                return;

            if (radioButtons != null)
            {
                foreach (var i in All.Int(radioButtons.Length))
                {
                    var r = radioButtons[i];
                    groupBox1.Controls.Remove(r);
                    r.Dispose();

                    var p = pictureBoxes[i];
                    // PictureBoxは必ず持っているとは限らない。
                    if (p != null)
                    {
                        groupBox1.Controls.Remove(p);
                        p.Dispose();
                    }

                    var img = images[i];
                    if (img != null)
                        img.Dispose();
                }
            }

            radioButtons = new Control[n];
            pictureBoxes = new Control[n];
            images = new ImageLoader[n];

            foreach (var i in All.Int(n))
            {
                var texts = SelectionTexts[i].Split(',');
                if (texts.Length != 2)
                    continue;

                var r = new RadioButton();
                var x = (pictureBox1.Width + groupBox1.Margin.Left*2) * i + groupBox1.Margin.Left;
                var rx = x + radioButton1.Location.X;
                r.Location = new Point(rx , radioButton1.Location.Y);
                r.Text = texts[0];
                var j = i; // copy for lambda's capture
                r.CheckedChanged += (sender, args) => { if (r.Checked) ViewModel.Selection = j; };
                r.Checked = i == ViewModel.Selection;
                radioButtons[i] = r;
                groupBox1.Controls.Add(r);

                var p = new PictureBox();
                var x2 = x;
                p.Location = new Point(x2 , pictureBox1.Location.Y);
                p.Size = pictureBox1.Size; // サイズは固定しておいたほうが扱いやすい
                pictureBoxes[i] = p;
                groupBox1.Controls.Add(p);

                var img = new ImageLoader();
                var tmp_img = new ImageLoader();
                var path = Path.Combine("image/display_setting/",texts[1]);
                tmp_img.Load(path);
                images[i] = tmp_img.CreateAndCopy(p.Width,p.Height);
                p.Image = images[i].image;
            }

            Invalidate();
        }

        /// <summary>
        /// 保持しているRadioButtonとPictureBox。
        /// SelectionTexts.Lengthの数だけ生成する。
        /// これらはgroupBox1にぶら下げて使う。
        /// </summary>
        private Control[] radioButtons;
        private Control[] pictureBoxes;
        private ImageLoader[] images;
        #endregion

        #region handlers
        private void RichSelector_SizeChanged(object sender, System.EventArgs e)
        {
            // サイズが変更されたら、それに合わせたGroupBoxのサイズに変更する。

            groupBox1.Size = new Size(Width - Margin.Size.Width*2 , Height - Margin.Size.Height*2);

            var n = SelectionTexts == null ? 0 : SelectionTexts.Length;
            if (n == 0)
                return;

            ResizeRadioButtons(n);

        }

        private void OnDispose(object sneder , EventArgs e)
        {
            if (images != null)
                foreach (var img in images)
                    img.Dispose();
        }
        #endregion
    }
}
