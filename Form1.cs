using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace TetrisGame
{
    public partial class TetrisGame : Form
    {

        public int interval = 500;//游戏速度
        public bool Running = false;    //游戏是否正在运行

        public int score = 0;   //得分

        public bool isPaused = false;   //游戏是否暂停&继续
        public int direction;       //方块的旋转方式
        public int myRotate = 1;    //当前方块的方向
        public int num = -1;        //判断是否首次生成随机方块
        public int speed = 1;       //每次下降的方格数

        public int classBlock = 0;  //当前方块的类型

        public int[] tempX = new int[10];   //存储每一列已放置的方块的高度

        /*存储了已放置&正在下落的方块的状态和颜色*/
        public static int[,] placedBlock = new int[23, 10];
        public static int[,] placedColor = new int[23, 10];
        public static int[,] status = new int[23, 10];
        public static int[,] color = new int[23, 10];
        public int[,] currentShape;
        public int[,] currentColor;

        public TetrisGame()
        {
            InitializeComponent();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (Running == false)
            {
                Console.WriteLine("游戏结束");
                Images.GameOver.Play(); //播放游戏失败音效
                label4.Visible = true;  //在窗口上显示游戏结束文字
                
                //显示最终得分
                label6.Text = "最终得分：" + score;  
                label6.Visible = true;

                暂停ToolStripMenuItem.Enabled = false;
                
                //分数清零
                score = 0;
                label1.Text = "得分：" + score;
                
                Timer1.Enabled = false;
                return;
            }
            else
            {
                //Console.WriteLine("开始计时");
                Down();     //下落方块
                pictureBox1.Refresh();  //重绘图像
            }

        }

        private void mainGamePaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            
            if (Running == true)
            {
                DrawBlock(g);
            }

        }

        //开始游戏
        private void StartGameClick(object sender, EventArgs e)
        {
            if (Running == false)
            {
                Running = true;
                开始ToolStripMenuItem.Enabled = false;

                Timer1.Interval = interval;
                RandomBlock();

                暂停ToolStripMenuItem.Enabled=true;
                Timer1.Stop();
                Timer1.Enabled = true;
                Timer1.Start();

                Console.WriteLine("Start");
                return;
            }
            else {}
            
        }

        //重新开始
        private void RestartGameClick(object sender, EventArgs e)
        {
            Application.Restart();
        }

        //暂停
        private void PauseGameClick(object sender, EventArgs e)
        {
            Pause();
        }

        //将方块显示在窗体中
        public void DrawBlock(Graphics g)
        {
            for (int y = 0; y < placedBlock.GetLength(0); y++)
            {
                for (int x = 0; x < placedBlock.GetLength(1); x++)
                {
                    if (currentShape[y, x] == 1)
                    {
                        //Console.WriteLine("[y,x]: {0},{1}", y, x);
                        g.DrawImage(Images.SoloBlocks[color[y, x]], x * 37, (y-3) * 37, 40, 40);
                        if (num > 0 && placedBlock[y, x] == 1)
                        {
                            g.DrawImage(Images.SoloBlocks[placedColor[y, x]], x * 37, (y - 3) * 37, 40, 40);
                        }
                    }
                }
            }
            num++;
            for (int y = 0; y < placedBlock.GetLength(0); y++)
            {
                for (int x = 0; x < placedBlock.GetLength(1); x++)
                {
                    if (placedBlock[y, x] == 1)
                    {
                        //Console.WriteLine("放置方块[y,x]: {0},{1}", y, x);
                        g.DrawImage(Images.SoloBlocks[placedColor[y, x]], x * 37, (y - 3) * 37, 40, 40);
                    }
                }
            }
        }

        //修改速度 慢 较慢 中等 较快 快
        private void SlowClick(object sender, EventArgs e)
        {
            interval = 1000;
        }

        private void LessSlowClick(object sender, EventArgs e)
        {
            interval = 700;
        }

        private void MediumClick(object sender, EventArgs e)
        {
            interval = 500;
        }

        private void FastClick(object sender, EventArgs e)
        {
            interval = 300;
        }

        private void FasterClick(object sender, EventArgs e)
        {
            interval = 100;
        }

        //判断是否到顶
        public bool IsTop()
        {
            for (int y = 0; y < currentShape.GetLength(0); y++)
            {
                for (int x = 0; x < currentShape.GetLength(1); x++)
                {
                    if (placedBlock[y, x] == 1/* && currentShape[y,x] != 1*/)
                    {
                        //Console.WriteLine("placedBlock [y,x]: {0},{1}", y, x);
                        tempX[x] = 23 - y;
                        if (tempX[x] >= 20) { return true; }
                        if (y <= 2) { return true; }
                    }
                }
            }
            return false;
        }

        //判断能否下落
        public bool IsDownEmpty()
        {
            int block = 1;
            for (int y = 0; y < currentShape.GetLength(0); y++)
            {
                for (int x = 0; x < currentShape.GetLength(1); x++)
                {
                    if (currentShape[y, x] == 1)
                    {
                        //Console.WriteLine(block);
                        if(block == 1)
                        {
                            if (y <= 21)
                            {
                                if (placedBlock[y + 1, x] == 1)
                                {
                                    return false;
                                }
                                else { }
                            }
                            else { return false; }
                        }
                        if (block == 2)
                        {
                            if (y <= 21)
                            {
                                if (placedBlock[y + 1, x] == 1)
                                {
                                    return false;
                                }
                                else { }
                            }
                            else { return false; }
                        }
                        if (block == 3)
                        {
                            if (y <= 21)
                            {
                                if (placedBlock[y + 1, x] == 1)
                                {
                                    return false;
                                }
                                else { }
                            }
                            else { return false; }
                        }
                        if (block == 4)
                        {
                            if (y <= 21)
                            {
                                if (placedBlock[y + 1, x] == 1)
                                {
                                    return false;
                                }
                                else { }
                            }
                            else { return false; }
                        }
                        
                    }
                }
            }
            return true;
        }

        //方块下落
        public void Down()
        {
            int[,] tempPos = new int[23, 10];
            int[,] tempColor = new int[23, 10];

            //如果到顶则结束游戏
            if (IsTop() == true)
            {
                Running = false;
            }

            //继续下落
            if (IsDownEmpty() == true)
            {

                for (int y = 0; y < currentShape.GetLength(0); y++)
                {
                    for (int x = 0; x < currentShape.GetLength(1); x++)
                    {
                        if (currentShape[y, x] == 1 /*&& y <= 21*/)
                        {
                            if (y <= 21)
                            {
                                //Console.WriteLine("下降");
                                //Console.WriteLine("[y,x]: {0},{1}", y, x);
                                tempPos[y + speed, x] = 1;
                                tempColor[y + speed, x] = classBlock;
                                status[y, x] = 0;
                            }
                            else
                            {
                                tempPos[y, x] = 1;
                                tempColor[y, x] = classBlock;
                            }

                            //Console.WriteLine("[y,x]: {0},{1}", y, x);                   
                        }
                        else { }
                    }
                }
                currentShape = tempPos;
                currentColor = tempColor;
                for (int y = 0; y < currentShape.GetLength(0); y++)
                {
                    for (int x = 0; x < currentShape.GetLength(1); x++)
                    {
                        if (currentShape[y, x] == 1)
                        {
                            //Console.WriteLine("[y,x]: {0},{1}", y, x);
                            status[y, x] = 1;
                            color[y, x] = currentColor[y, x];
                        }

                    }
                }
            }
            else  //如果不能下落则执行放置方块的操作
            {
                //Console.WriteLine("放置");
                Images.BlockDown.Play();    //播放放置方块的音效
                Timer1.Interval = interval;
                myRotate = 1;
                for (int y = 0; y < currentShape.GetLength(0); y++)
                {
                    for (int x = 0; x < currentShape.GetLength(1); x++)
                    {
                        if (currentShape[y, x] == 1)
                        {
                            placedBlock[y, x] = 1;
                            placedColor[y, x] = classBlock;
                        }
                    }
                }
                FullLines();    //检测是否满行并进行消除

                RandomBlock();  //生成下一个方块
            }
        }

        //检测是否满行并消除
        public void FullLines()
        {
            int[,] tempPos = new int[23, 10];
            int[,] tempColor = new int[23, 10];
            int numOfLines = 0; //行满个数
            bool isFull = false;
            tempPos = placedBlock;
            tempColor = placedColor;
            for (int y = placedBlock.GetLength(0) - 1; y > 0; y--)
            {
                int num = 0;
                for (int x = 0; x < placedBlock.GetLength(1); x++)
                {
                    if (placedBlock[y, x] == 1)
                    {
                        num++;
                    }
                }
                if (num >= 10)   //该行每个位置都有方块
                {
                    isFull = true;
                    numOfLines++;
                }
            }

            //消除1行，得10分；消除2行，得20分；消除3行，得40分；消除4行，得80分
            if(numOfLines == 1) { score += 10; }
            else if(numOfLines == 2) { score += 20; }
            else if(numOfLines == 3) { score += 40; }
            else if(numOfLines == 4) { score += 80; }
            else { }
            label1.Text = "得分：" + score;    //更新分数

            //去除满行并将上一行的方块移动到这一行
            for (int y = 0; y < placedBlock.GetLength(0); y++)
            {
                int num = 0;
                for (int x = 0; x < placedBlock.GetLength(1); x++)
                {
                    if (placedBlock[y, x] == 1)
                    {
                        num++;
                    }
                    if(num >= 10)
                    {
                        for (int placedY = y - 1; placedY > 0; placedY--)
                        {
                            for (int placedX = 0; placedX < placedBlock.GetLength(1); placedX++)
                            {
                                tempPos[placedY + 1, placedX] = tempPos[placedY, placedX];
                                tempColor[placedY + 1, placedX] = tempColor[placedY, placedX];
                                tempPos[placedY, placedX] = 0;
                                tempColor[placedY, placedX] = 0;
                            }
                        }
                    }
                }
            }

            if(isFull == true)
            {
                placedBlock = tempPos;
                placedColor = tempColor;
                Images.EraseLines.Play();   //播放消行的音效
            }
            
        }

        //能否右移
        public bool IsRight()
        {
            int maxX = 0;
            for (int y = 0; y < currentShape.GetLength(0); y++)
            {
                for (int x = 0; x < currentShape.GetLength(1); x++)
                {
                    if (currentShape[y, x] == 1)
                    {
                        if (maxX < x) { maxX = x; }
                    }
                }
            }
            for (int y = 0; y < currentShape.GetLength(0); y++)
            {
                for (int x = 0; x < currentShape.GetLength(1); x++)
                {
                    if ((currentShape[y, x] == 1 && placedBlock[y, x + 1] == 1) || (maxX) >= placedBlock.GetLength(1) - 1)
                    {
                        //Console.WriteLine("不能右移");
                        //Console.WriteLine("[y,x]: {0},{1}", y, x);
                        return false;
                    }
                }
            }

            return true;
        }

        //向右移动
        public void right()
        {
            int[,] tempPos = new int[23, 10];
            int[,] tempColor = new int[23, 10];
            if (IsRight())
            {
                //Console.WriteLine("可以右移");
                for (int y = 0; y < currentShape.GetLength(0); y++)
                {
                    for (int x = 0; x < currentShape.GetLength(1); x++)
                    {
                        if (currentShape[y, x] == 1)
                        {
                            //Console.WriteLine("右移后 [y,x]: {0},{1}", y, x);
                            tempPos[y, x + 1] = 1;
                            tempColor[y, x + 1] = classBlock;
                            status[y, x] = 0;
                        }
                    }
                }
                currentShape = tempPos;
                currentColor = tempColor;
                for (int y = 0; y < currentShape.GetLength(0); y++)
                {
                    for (int x = 0; x < currentShape.GetLength(1); x++)
                    {
                        if (currentShape[y, x] == 1)
                        {
                            //Console.WriteLine("[y,x]: {0},{1}", y, x);
                            status[y, x] = 1;
                            color[y, x] = currentColor[y, x];
                        }

                    }
                }
            }
            else { }
        }

        //能否左移
        public bool IsLeft()
        {
            int minX = 10;
            for (int y = 0; y < currentShape.GetLength(0); y++)
            {
                for (int x = 0; x < currentShape.GetLength(1); x++)
                {
                    if (currentShape[y, x] == 1)
                    {
                        if (minX > x) { minX = x; }
                    }
                }
            }
            for (int y = 0; y < currentShape.GetLength(0); y++)
            {
                for (int x = 0; x < currentShape.GetLength(1); x++)
                {
                    if ((currentShape[y, x] == 1 && placedBlock[y, x - 1] == 1) || (minX) < 1)
                    {
                        //Console.WriteLine("不能左移");
                        //Console.WriteLine("[y,x]: {0},{1}", y, x);
                        return false;
                    }
                }
            }
            return true;
        }
            
        //向左移动
        public void left()
        {
            int[,] tempPos = new int[23, 10];
            int[,] tempColor = new int[23, 10];
            if (IsLeft())
            {
                //Console.WriteLine("可以左移");
                for (int y = 0; y < currentShape.GetLength(0); y++)
                {
                    for (int x = 0; x < currentShape.GetLength(1); x++)
                    {
                        if (currentShape[y, x] == 1)
                        {
                            //Console.WriteLine("左移后 [y,x]: {0},{1}", y, x);
                            tempPos[y, x - 1] = 1;
                            tempColor[y, x - 1] = classBlock;
                            status[y, x] = 0;
                        }
                    }
                }
                currentShape = tempPos;
                currentColor = tempColor;
                for (int y = 0; y < currentShape.GetLength(0); y++)
                {
                    for (int x = 0; x < currentShape.GetLength(1); x++)
                    {
                        if (currentShape[y, x] == 1)
                        {
                            //Console.WriteLine("[y,x]: {0},{1}", y, x);
                            status[y, x] = 1;
                            color[y, x] = currentColor[y, x];
                        }

                    }
                }
            }
            else { }
        }

        //旋转(逆时针)
        public void Rotate(int classBlock)
        {
            direction = 0;
            switch(classBlock)  //根据不同方块类型设置不同的旋转次数
            {
                case 0: //I、S、Z 2种旋转
                case 3:
                case 4:
                    direction = 2;
                    break;
                case 1: //O 1种旋转
                    direction = 1;
                    break;
                case 2: //T、J、L 4种旋转   
                case 5:
                case 6:
                    direction = 4;
                    break;
                default: direction = 1; break;
            }
            
            //对方块进行旋转操作
            SetRotation(direction);
            
        }

        //不同种类的方块有不同的旋转方式
        public void SetRotation(int direction)
        {
            //Console.WriteLine(myRotate);
            int[,] tempPos = new int[23, 10];
            int[,] tempColor = new int[23, 10];
            int block = 1;

            /*以逐行扫描的方式，顺序遍历一种方块的四个小方块，
              根据方块当前的方向来改变四个小方块的位置以达到旋转的目的*/
            if (myRotate <= direction)
            {
                switch (classBlock)
                {
                    case 0: //I 2
                        switch (myRotate)
                        {
                            case 1:
                                for (int y = 0; y < currentShape.GetLength(0); y++)
                                {
                                    for (int x = 0; x < currentShape.GetLength(1); x++)
                                    {
                                        if (currentShape[y, x] == 1)
                                        {
                                            //Console.WriteLine("[y,x]: {0},{1}", y, x);
                                            if (block == 1)
                                            {
                                                if (y <= 19 && (x <= 8))
                                                {
                                                    tempPos[y - 1, x + 2] = 1;
                                                    tempColor[y - 1, x + 2] = classBlock;
                                                }
                                                else { return; }
                                            }
                                            else if (block == 2)
                                            {
                                                tempPos[y, x + 1] = 1;
                                                tempColor[y, x + 1] = classBlock;
                                            }
                                            else if (block == 3)
                                            {
                                                tempPos[y + 1, x] = 1;
                                                tempColor[y + 1, x] = classBlock;
                                            }
                                            else if (block == 4)
                                            {
                                                tempPos[y + 2, x - 1] = 1;
                                                tempColor[y + 2, x - 1] = classBlock;
                                                break;
                                            }
                                            block++;
                                        }
                                    }
                                }
                                break;
                            case 2:
                                for (int y = 0; y < currentShape.GetLength(0); y++)
                                {
                                    for (int x = 0; x < currentShape.GetLength(1); x++)
                                    {
                                        if (currentShape[y, x] == 1/* && (x >= 2 && x <= 8)*/)
                                        {
                                            //Console.WriteLine("[y,x]: {0},{1}", y, x);
                                            if (block == 1)
                                            {
                                                if (x >= 2 && x <= 8)
                                                {
                                                    tempPos[y + 1, x - 2] = 1;
                                                    tempColor[y + 1, x - 2] = classBlock;
                                                }
                                                else { return; }
                                            }
                                            else if (block == 2)
                                            {
                                                tempPos[y, x - 1] = 1;
                                                tempColor[y, x - 1] = classBlock;
                                            }
                                            else if (block == 3)
                                            {
                                                tempPos[y - 1, x] = 1;
                                                tempColor[y - 1, x] = classBlock;
                                            }
                                            else if (block == 4)
                                            {
                                                tempPos[y - 2, x + 1] = 1;
                                                tempColor[y - 2, x + 1] = classBlock;
                                                break;
                                            }
                                            block++;
                                        }
                                    }
                                }

                                break;
                        }
                        break;

                    case 1: //O 1
                        return;

                    case 2: //T 4
                        switch (myRotate)
                        {
                            case 1:
                                //Console.WriteLine(myRotate);
                                for (int y = 0; y < currentShape.GetLength(0); y++)
                                {
                                    for (int x = 0; x < currentShape.GetLength(1); x++)
                                    {
                                        if (currentShape[y, x] == 1)
                                        {
                                            if (block == 1)
                                            {
                                                tempPos[y - 1, x] = 1;
                                                tempColor[y - 1, x] = classBlock;
                                            }
                                            else if (block == 2)
                                            {
                                                tempPos[y - 1, x] = 1;
                                                tempColor[y - 1, x] = classBlock;
                                            }
                                            else if (block == 3)
                                            {
                                                tempPos[y - 1, x] = 1;
                                                tempColor[y - 1, x] = classBlock;
                                            }
                                            else if (block == 4)
                                            {
                                                tempPos[y, x - 1] = 1;
                                                tempColor[y, x - 1] = classBlock;
                                                break;
                                            }
                                            block++;
                                        }
                                    }
                                }
                                break;
                            case 2:
                                for (int y = 0; y < currentShape.GetLength(0); y++)
                                {
                                    for (int x = 0; x < currentShape.GetLength(1); x++)
                                    {
                                        if (currentShape[y, x] == 1)
                                        {
                                            if (block == 1)
                                            {
                                                if (x <= 8)
                                                {
                                                    tempPos[y + 1, x - 1] = 1;
                                                    tempColor[y + 1, x - 1] = classBlock;
                                                }
                                                else { return; }
                                            }
                                            else if (block == 2)
                                            {
                                                tempPos[y, x + 1] = 1;
                                                tempColor[y, x + 1] = classBlock;
                                            }
                                            else if (block == 3)
                                            {
                                                tempPos[y, x + 1] = 1;
                                                tempColor[y, x + 1] = classBlock;
                                            }
                                            else if (block == 4)
                                            {
                                                tempPos[y, x] = 1;
                                                tempColor[y, x] = classBlock;
                                                break;
                                            }
                                            block++;
                                        }
                                    }
                                }
                                break;
                            case 3:
                                for (int y = 0; y < currentShape.GetLength(0); y++)
                                {
                                    for (int x = 0; x < currentShape.GetLength(1); x++)
                                    {
                                        if (currentShape[y, x] == 1)
                                        {
                                            if (block == 1)
                                            {
                                                tempPos[y - 1, x + 1] = 1;
                                                tempColor[y - 1, x + 1] = classBlock;
                                            }
                                            else if (block == 2)
                                            {
                                                tempPos[y, x] = 1;
                                                tempColor[y, x] = classBlock;
                                            }
                                            else if (block == 3)
                                            {
                                                tempPos[y, x] = 1;
                                                tempColor[y, x] = classBlock;
                                            }
                                            else if (block == 4)
                                            {
                                                tempPos[y, x] = 1;
                                                tempColor[y, x] = classBlock;
                                                break;
                                            }
                                            block++;
                                        }
                                    }
                                }
                                break;
                            case 4:
                                for (int y = 0; y < currentShape.GetLength(0); y++)
                                {
                                    for (int x = 0; x < currentShape.GetLength(1); x++)
                                    {
                                        if (currentShape[y, x] == 1)
                                        {
                                            if (block == 1)
                                            {
                                                if (x >= 1)
                                                {
                                                    tempPos[y + 1, x] = 1;
                                                    tempColor[y + 1, x] = classBlock;
                                                }
                                                else { return; }
                                            }
                                            else if (block == 2)
                                            {
                                                tempPos[y + 1, x - 1] = 1;
                                                tempColor[y + 1, x - 1] = classBlock;
                                            }
                                            else if (block == 3)
                                            {
                                                tempPos[y + 1, x - 1] = 1;
                                                tempColor[y + 1, x - 1] = classBlock;
                                            }
                                            else if (block == 4)
                                            {
                                                tempPos[y, x + 1] = 1;
                                                tempColor[y, x + 1] = classBlock;
                                                break;
                                            }
                                            block++;
                                        }
                                    }
                                }
                                break;
                        }
                        break;

                    case 3: //S 2
                        switch (myRotate)
                        {
                            case 1:
                                for (int y = 0; y < currentShape.GetLength(0); y++)
                                {
                                    for (int x = 0; x < currentShape.GetLength(1); x++)
                                    {
                                        if (currentShape[y, x] == 1)
                                        {
                                            if (block == 1)
                                            {
                                                tempPos[y - 1, x - 1] = 1;
                                                tempColor[y - 1, x - 1] = classBlock;
                                            }
                                            else if (block == 2)
                                            {
                                                tempPos[y, x - 2] = 1;
                                                tempColor[y, x - 2] = classBlock;
                                            }
                                            else if (block == 3)
                                            {
                                                tempPos[y - 1, x + 1] = 1;
                                                tempColor[y - 1, x + 1] = classBlock;
                                            }
                                            else if (block == 4)
                                            {
                                                tempPos[y, x] = 1;
                                                tempColor[y, x] = classBlock;
                                                break;
                                            }
                                            block++;
                                        }
                                    }
                                }
                                break;
                            case 2:
                                for (int y = 0; y < currentShape.GetLength(0); y++)
                                {
                                    for (int x = 0; x < currentShape.GetLength(1); x++)
                                    {
                                        if (currentShape[y, x] == 1)
                                        {
                                            if (block == 1)
                                            {
                                                if (x <= 7)
                                                {
                                                    tempPos[y + 1, x + 1] = 1;
                                                    tempColor[y + 1, x + 1] = classBlock;
                                                }
                                                else { return; }
                                            }
                                            else if (block == 2)
                                            {
                                                tempPos[y, x + 2] = 1;
                                                tempColor[y, x + 2] = classBlock;
                                            }
                                            else if (block == 3)
                                            {
                                                tempPos[y + 1, x - 1] = 1;
                                                tempColor[y + 1, x - 1] = classBlock;
                                            }
                                            else if (block == 4)
                                            {
                                                tempPos[y, x] = 1;
                                                tempColor[y, x] = classBlock;
                                                break;
                                            }
                                            block++;
                                        }
                                    }
                                }
                                break;
                        }
                        break;

                    case 4: //Z 2
                        switch (myRotate)
                        {
                            case 1:
                                for (int y = 0; y < currentShape.GetLength(0); y++)
                                {
                                    for (int x = 0; x < currentShape.GetLength(1); x++)
                                    {
                                        if (currentShape[y, x] == 1)
                                        {
                                            if (block == 1)
                                            {
                                                tempPos[y - 1, x + 2] = 1;
                                                tempColor[y - 1, x + 2] = classBlock;
                                            }
                                            else if (block == 2)
                                            {
                                                tempPos[y, x] = 1;
                                                tempColor[y, x] = classBlock;
                                            }
                                            else if (block == 3)
                                            {
                                                tempPos[y - 1, x + 1] = 1;
                                                tempColor[y - 1, x + 1] = classBlock;
                                            }
                                            else if (block == 4)
                                            {
                                                tempPos[y, x - 1] = 1;
                                                tempColor[y, x - 1] = classBlock;
                                                break;
                                            }
                                            block++;
                                        }
                                    }
                                }
                                break;
                            case 2:
                                for (int y = 0; y < currentShape.GetLength(0); y++)
                                {
                                    for (int x = 0; x < currentShape.GetLength(1); x++)
                                    {
                                        if (currentShape[y, x] == 1)
                                        {
                                            if (block == 1)
                                            {
                                                if (x >= 2)
                                                {
                                                    tempPos[y + 1, x - 2] = 1;
                                                    tempColor[y + 1, x - 2] = classBlock;
                                                }
                                                else { return; }
                                            }
                                            else if (block == 2)
                                            {
                                                tempPos[y, x] = 1;
                                                tempColor[y, x] = classBlock;
                                            }
                                            else if (block == 3)
                                            {
                                                tempPos[y + 1, x - 1] = 1;
                                                tempColor[y + 1, x - 1] = classBlock;
                                            }
                                            else if (block == 4)
                                            {
                                                tempPos[y, x + 1] = 1;
                                                tempColor[y, x + 1] = classBlock;
                                                break;
                                            }
                                            block++;
                                        }
                                    }
                                }
                                break;
                        }
                        break;

                    case 5: //J 4
                        switch (myRotate)
                        {
                            case 1:
                                for (int y = 0; y < currentShape.GetLength(0); y++)
                                {
                                    for (int x = 0; x < currentShape.GetLength(1); x++)
                                    {
                                        if (currentShape[y, x] == 1)
                                        {
                                            if (block == 1)
                                            {
                                                tempPos[y - 1, x + 1] = 1;
                                                tempColor[y - 1, x + 1] = classBlock;
                                            }
                                            else if (block == 2)
                                            {
                                                tempPos[y - 1, x + 1] = 1;
                                                tempColor[y - 1, x + 1] = classBlock;
                                            }
                                            else if (block == 3)
                                            {
                                                tempPos[y, x - 1] = 1;
                                                tempColor[y, x - 1] = classBlock;
                                            }
                                            else if (block == 4)
                                            {
                                                tempPos[y, x - 1] = 1;
                                                tempColor[y, x - 1] = classBlock;
                                                break;
                                            }
                                            block++;
                                        }
                                    }
                                }
                                break;

                            case 2:
                                for (int y = 0; y < currentShape.GetLength(0); y++)
                                {
                                    for (int x = 0; x < currentShape.GetLength(1); x++)
                                    {
                                        if (currentShape[y, x] == 1)
                                        {
                                            if (block == 1)
                                            {
                                                if (x <= 8)
                                                {
                                                    tempPos[y + 1, x - 1] = 1;
                                                    tempColor[y + 1, x - 1] = classBlock;
                                                }
                                                else { return; }
                                            }
                                            else if (block == 2)
                                            {
                                                tempPos[y, x] = 1;
                                                tempColor[y, x] = classBlock;
                                            }
                                            else if (block == 3)
                                            {
                                                tempPos[y - 1, x + 2] = 1;
                                                tempColor[y - 1, x + 2] = classBlock;
                                            }
                                            else if (block == 4)
                                            {
                                                tempPos[y, x + 1] = 1;
                                                tempColor[y, x + 1] = classBlock;
                                                break;
                                            }
                                            block++;
                                        }
                                    }
                                }
                                break;

                            case 3:
                                for (int y = 0; y < currentShape.GetLength(0); y++)
                                {
                                    for (int x = 0; x < currentShape.GetLength(1); x++)
                                    {
                                        if (currentShape[y, x] == 1)
                                        {
                                            if (block == 1)
                                            {
                                                tempPos[y - 1, x + 1] = 1;
                                                tempColor[y - 1, x + 1] = classBlock;
                                            }
                                            else if (block == 2)
                                            {
                                                tempPos[y - 1, x + 1] = 1;
                                                tempColor[y - 1, x + 1] = classBlock;
                                            }
                                            else if (block == 3)
                                            {
                                                tempPos[y, x - 1] = 1;
                                                tempColor[y, x - 1] = classBlock;
                                            }
                                            else if (block == 4)
                                            {
                                                tempPos[y, x - 1] = 1;
                                                tempColor[y, x - 1] = classBlock;
                                                break;
                                            }
                                            block++;
                                        }
                                    }
                                }
                                break;

                            case 4:
                                for (int y = 0; y < currentShape.GetLength(0); y++)
                                {
                                    for (int x = 0; x < currentShape.GetLength(1); x++)
                                    {
                                        if (currentShape[y, x] == 1)
                                        {
                                            if (block == 1)
                                            {
                                                if (x >= 1)
                                                {
                                                    tempPos[y + 1, x - 1] = 1;
                                                    tempColor[y + 1, x - 1] = classBlock;
                                                }
                                                else { return; }
                                            }
                                            else if (block == 2)
                                            {
                                                tempPos[y + 2, x - 2] = 1;
                                                tempColor[y + 2, x - 2] = classBlock;
                                            }
                                            else if (block == 3)
                                            {
                                                tempPos[y + 1, x] = 1;
                                                tempColor[y + 1, x] = classBlock;
                                            }
                                            else if (block == 4)
                                            {
                                                tempPos[y, x + 1] = 1;
                                                tempColor[y, x + 1] = classBlock;
                                                break;
                                            }
                                            block++;
                                        }
                                    }
                                }
                                break;
                        }
                        break;

                    case 6: //L 4
                        switch (myRotate)
                        {
                            case 1:
                                for (int y = 0; y < currentShape.GetLength(0); y++)
                                {
                                    for (int x = 0; x < currentShape.GetLength(1); x++)
                                    {
                                        if (currentShape[y, x] == 1)
                                        {
                                            if (block == 1)
                                            {
                                                tempPos[y - 1, x - 2] = 1;
                                                tempColor[y - 1, x - 2] = classBlock;
                                            }
                                            else if (block == 2)
                                            {
                                                tempPos[y - 2, x + 1] = 1;
                                                tempColor[y - 2, x + 1] = classBlock;
                                            }
                                            else if (block == 3)
                                            {
                                                tempPos[y - 1, x] = 1;
                                                tempColor[y - 1, x] = classBlock;
                                            }
                                            else if (block == 4)
                                            {
                                                tempPos[y, x - 1] = 1;
                                                tempColor[y, x - 1] = classBlock;
                                                break;
                                            }
                                            block++;
                                        }
                                    }
                                }
                                break;

                            case 2:
                                for (int y = 0; y < currentShape.GetLength(0); y++)
                                {
                                    for (int x = 0; x < currentShape.GetLength(1); x++)
                                    {
                                        if (currentShape[y, x] == 1)
                                        {
                                            if (block == 1)
                                            {
                                                if (x <= 7)
                                                {
                                                    tempPos[y + 1, x] = 1;
                                                    tempColor[y + 1, x] = classBlock;
                                                }
                                                else { return; }
                                            }
                                            else if (block == 2)
                                            {
                                                tempPos[y + 1, x] = 1;
                                                tempColor[y + 1, x] = classBlock;
                                            }
                                            else if (block == 3)
                                            {
                                                tempPos[y, x + 1] = 1;
                                                tempColor[y, x + 1] = classBlock;
                                            }
                                            else if (block == 4)
                                            {
                                                tempPos[y, x - 1] = 1;
                                                tempColor[y, x - 1] = classBlock;
                                                break;
                                            }
                                            block++;
                                        }
                                    }
                                }
                                break;

                            case 3:
                                for (int y = 0; y < currentShape.GetLength(0); y++)
                                {
                                    for (int x = 0; x < currentShape.GetLength(1); x++)
                                    {
                                        if (currentShape[y, x] == 1)
                                        {
                                            if (block == 1)
                                            {
                                                tempPos[y - 1, x + 1] = 1;
                                                tempColor[y - 1, x + 1] = classBlock;
                                            }
                                            else if (block == 2)
                                            {
                                                tempPos[y, x] = 1;
                                                tempColor[y, x] = classBlock;
                                            }
                                            else if (block == 3)
                                            {
                                                tempPos[y + 1, x - 1] = 1;
                                                tempColor[y + 1, x - 1] = classBlock;
                                            }
                                            else if (block == 4)
                                            {
                                                tempPos[y, x + 2] = 1;
                                                tempColor[y, x + 2] = classBlock;
                                                break;
                                            }
                                            block++;
                                        }
                                    }
                                }
                                break;

                            case 4:
                                for (int y = 0; y < currentShape.GetLength(0); y++)
                                {
                                    for (int x = 0; x < currentShape.GetLength(1); x++)
                                    {
                                        if (currentShape[y, x] == 1)
                                        {
                                            if (block == 1)
                                            {
                                                if (x >= 1)
                                                {
                                                    tempPos[y + 1, x + 1] = 1;
                                                    tempColor[y + 1, x + 1] = classBlock;
                                                }
                                                else { return; }
                                            }
                                            else if (block == 2)
                                            {
                                                tempPos[y + 1, x - 1] = 1;
                                                tempColor[y + 1, x - 1] = classBlock;
                                            }
                                            else if (block == 3)
                                            {
                                                tempPos[y, x] = 1;
                                                tempColor[y, x] = classBlock;
                                            }
                                            else if (block == 4)
                                            {
                                                tempPos[y, x] = 1;
                                                tempColor[y, x] = classBlock;
                                                break;
                                            }
                                            block++;
                                        }
                                    }
                                }
                                break;
                        }
                        break;

                }
                //myRotate++;
            }
            else { }

            /*能否旋转。如果旋转后有一个小方块的位置与已有方块重合，那么就不能旋转*/
            for (int y = 0; y < currentShape.GetLength(0); y++)
            {
                for (int x = 0; x < currentShape.GetLength(1); x++)
                {
                    if (tempPos[y, x] == 1 && placedBlock[y, x] == 1)
                    {
                        return;
                    }
                }
            }

            /*如果可以旋转，则改变当前方块的方向*/
            if (myRotate >= direction) { myRotate = 1; }
            else { myRotate++; }
            status = tempPos;
            color = tempColor;
            currentShape = tempPos;
            currentColor = tempColor;
        }

        //暂停或继续游戏
        public void Pause()
        {
            isPaused = !isPaused;
            if(isPaused == true)
            {
                Timer1.Enabled = false;
                暂停ToolStripMenuItem.Text = "继续";
            }
            else
            {
                Timer1.Enabled = true;
                暂停ToolStripMenuItem.Text = "暂停";
            }
        }

        //按键事件
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Running == true)
            {
                switch (e.KeyCode)
                {
                    case Keys.Right: //右：向右移动
                        right();
                        break;

                    case Keys.Left:  //左：向左移动
                        left();
                        break;

                    case Keys.Up:   //上：旋转
                        Rotate(classBlock);
                        break;

                    case Keys.Down: //下：向下加速
                        Timer1.Interval = 10;
                        break; 

                    case Keys.Space:    //空格：暂停
                        Pause();
                        break;

                    default: break;
                }
            }

        }

        /// <summary>
        /// 生成随机方块
        /// </summary>
        public void RandomBlock()
        {
            Random random = new Random();
            classBlock = random.Next(0, 7);
            SetShape(classBlock);
            SetColor(classBlock);

            for (int y = 0; y < currentShape.GetLength(0); y++)
            {
                for (int x = 0; x < currentShape.GetLength(1); x++)
                {
                    if (currentShape[y, x] == 1)
                    {
                        status[y, x] = 1;
                        color[y, x] = currentColor[y, x];
                    }

                }
            }
        }

        /// <summary>
        /// 设置当前方块初始的位置
        /// </summary>
        /// <param name="i">方块的类型</param>
        public void SetShape(int i)
        {
            switch (i)
            {
                case (0):
                    currentShape = new int[23, 10];
                    currentShape[3, 3] = 1;
                    currentShape[3, 4] = 1;
                    currentShape[3, 5] = 1;
                    currentShape[3, 6] = 1;
                    break;
                case (1):
                    currentShape = new int[23, 10];
                    currentShape[3, 4] = 1;
                    currentShape[3, 5] = 1;
                    currentShape[4, 4] = 1;
                    currentShape[4, 5] = 1;
                    break;
                case (2):
                    currentShape = new int[23, 10];
                    currentShape[3, 4] = 1;
                    currentShape[4, 3] = 1;
                    currentShape[4, 4] = 1;
                    currentShape[4, 5] = 1;
                    break;
                case (3):
                    currentShape = new int[23, 10];
                    currentShape[3, 4] = 1;
                    currentShape[3, 5] = 1;
                    currentShape[4, 3] = 1;
                    currentShape[4, 4] = 1;
                    break;
                case (4):
                    currentShape = new int[23, 10];
                    currentShape[3, 3] = 1;
                    currentShape[3, 4] = 1;
                    currentShape[4, 4] = 1;
                    currentShape[4, 5] = 1;
                    break;
                case (5):
                    currentShape = new int[23, 10];
                    currentShape[3, 3] = 1;
                    currentShape[4, 3] = 1;
                    currentShape[4, 4] = 1;
                    currentShape[4, 5] = 1;
                    break;
                case (6):
                    currentShape = new int[23, 10];
                    currentShape[3, 5] = 1;
                    currentShape[4, 3] = 1;
                    currentShape[4, 4] = 1;
                    currentShape[4, 5] = 1;
                    break;
                default: currentShape = new int[23, 10]; break;
            }
        }

        /// <summary>
        /// 设置当前方块的颜色
        /// </summary>
        /// <param name="i">方块的类型</param>
        public void SetColor(int i)
        {
            switch (i)
            {
                case (0):
                    currentColor = new int[23, 10];
                    currentColor[3, 3] = 0;
                    currentColor[3, 4] = 0;
                    currentColor[3, 5] = 0;
                    currentColor[3, 6] = 0;
                    break;
                case (1):
                    currentColor = new int[23, 10];
                    currentColor[3, 4] = 1;
                    currentColor[3, 5] = 1;
                    currentColor[4, 4] = 1;
                    currentColor[4, 5] = 1;
                    break;
                case (2):
                    currentColor = new int[23, 10];
                    currentColor[3, 4] = 2;
                    currentColor[4, 3] = 2;
                    currentColor[4, 4] = 2;
                    currentColor[4, 5] = 2;
                    break;
                case (3):
                    currentColor = new int[23, 10];
                    currentColor[3, 4] = 3;
                    currentColor[3, 5] = 3;
                    currentColor[4, 3] = 3;
                    currentColor[4, 4] = 3;
                    break;
                case (4):
                    currentColor = new int[23, 10];
                    currentColor[3, 3] = 4;
                    currentColor[3, 4] = 4;
                    currentColor[4, 4] = 4;
                    currentColor[4, 5] = 4;
                    break;
                case (5):
                    currentColor = new int[23, 10];
                    currentColor[3, 3] = 5;
                    currentColor[4, 3] = 5;
                    currentColor[4, 4] = 5;
                    currentColor[4, 5] = 5;
                    break;
                case (6):
                    currentColor = new int[23, 10];
                    currentColor[3, 5] = 6;
                    currentColor[4, 3] = 6;
                    currentColor[4, 4] = 6;
                    currentColor[4, 5] = 6;
                    break;
                default: currentColor = new int[23, 10]; break;
            }
        }

    }
} 
