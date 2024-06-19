using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace TetrisGame
{
    static internal class Images
    {
        //存储了7种方块的图像
        public static Image[] SoloBlocks = new Image[7]
        {
            Image.FromFile("images\\Solo_Block_I.png"),
            Image.FromFile("images\\Solo_Block_O.png"),
            Image.FromFile("images\\Solo_Block_T.png"),
            Image.FromFile("images\\Solo_Block_S.png"),
            Image.FromFile("images\\Solo_Block_Z.png"),
            Image.FromFile("images\\Solo_Block_7.png"),
            Image.FromFile("images\\Solo_Block_L.png"),
        };

        //游戏失败音效
        public static SoundPlayer GameOver = new SoundPlayer("images\\GameOver.wav");
        //方块下落音效
        public static SoundPlayer BlockDown = new SoundPlayer("images\\BlockDown.wav");
        //消行音效
        public static SoundPlayer EraseLines = new SoundPlayer("images\\EraseLines.wav");
    }
}