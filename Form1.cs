using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Threading;

namespace halcon_test
{
    public partial class Form1 : Form
    {
        private HFramegrabber Framegrabber;
        private HImage Img;
        private HDevelopExport de;
        private PictureBox pictureBox;
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private Result result;



        public Form1()
        {
          
            InitializeComponent();
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            // 获取可用的视频设备
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            // 检查是否有可用的摄像头
            if (videoDevices.Count > 0)
            {
                // 创建VideoCaptureDevice对象
                videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                // 设置NewFrame事件处理方法
                videoSource.NewFrame += VideoSource_NewFrame;
                // 启动视频流
                videoSource.Start();
            }
            else
            {
                MessageBox.Show("未检测到可用摄像头。");
            }
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // 在PictureBox上显示图像
            pictureBox1.Image = (System.Drawing.Image)eventArgs.Frame.Clone();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            de = new HDevelopExport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Image image = Image.FromFile(filepath_text.Text);
                pictureBox2.Image = image;
                pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
                string[] res = de.decode(pictureBox2);
                label1.Text = res[0];
                //de.DisposeWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Interval = 1000;
            timer1.Start();
            button2.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var  res =  de.decode(pictureBox1);
            if (res.Length != 0)
            {
                label1.Text = res[0].ToString();
            }
            else
            {
                label1.Text = "未识别到条形码";
                de.DisposeWindow();
            }
        }
    }


}
