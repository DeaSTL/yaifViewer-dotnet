using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace yaifViewer
{
    public partial class ImageViewer : Form
    {
        private int image_width;
        private int image_height;
        private int palette_size;
        private int image_size;
        private int file_size;
        private int image_start;
        private int palette_start;
        private int[,] palette;
        private int[,] image;

        public Graphics graphics;
        public int get32BitInt(int offset)
        {
            return Program.inputFile[offset] + (Program.inputFile[offset + 1] << 8) + (Program.inputFile[offset + 2] << 16) +
                (Program.inputFile[offset + 3] << 32);
        }

        public int get16BitInt(int offset)
        {
            return Program.inputFile[offset] + (Program.inputFile[offset + 1] << 8);
        }

        public int get8BitInt(int offset)
        {
            return Program.inputFile[offset];
        }

        public int[] getNibbles(int offset)
        {
           
            int n1 = Program.inputFile[offset] & 0x0f;
            int n2 = Program.inputFile[offset] >> 4;
            int[] nibbles = { n1, n2 };
            return nibbles;
            
            
        }
        public int[,] getImage() {
            int[] dc_image = new int[image_width * image_height];
            for (int i = 0; i < image_size*2; i += 2) {
                int[] nibbles = getNibbles((i/2) + image_start);
                dc_image[i] = nibbles[1];
                dc_image[i + 1] = nibbles[0];
            }
            int[,] output = new int[image_width, image_height];
            for (int x = 0; x < image_width; x++) {
                for (int y = 0; y < image_height; y++) {
                    output[x, y] = dc_image[x + image_width * y];
                    
                }

            }
            return output;
        }
        public int[,] getPalette() {
            int[,] output = new int[palette_size / 3, 3];
            for (int i = 0; i < palette_size; i+=3) {
                output[i / 3, 0] = get8BitInt(palette_start + i);
                output[i / 3, 1] = get8BitInt(palette_start + i + 1);
                output[i / 3, 2] = get8BitInt(palette_start + i + 2);

            }
            return output;


        }

        public ImageViewer()
        {
            InitializeComponent();
            
            this.DoubleBuffered = true;
            image_width = get16BitInt(0);
            image_height = get16BitInt(2);
            palette_size = get8BitInt(4);
            image_size = get32BitInt(5);
            file_size = get32BitInt(9);
            palette_start = 13;
            image_start = palette_start + palette_size;
            ImageBox.Image = new Bitmap(image_width, image_height);
            
            palette = getPalette();
            image = getImage();
            Console.WriteLine(image_width * image_height);
            Console.WriteLine(image.Length);
            for (int x = 0; x < image_width; x++)
            {
                for (int y = 0; y < image_height; y++)
                {
                    int current_pixel = image[x, y];
                   
                    ((Bitmap)ImageBox.Image).SetPixel(x, y, Color.FromArgb(
                        palette[current_pixel, 0],
                        palette[current_pixel, 1],
                        palette[current_pixel, 2]
                        )
                        );

                }

            }
            ImageBox.Invalidate();
            ImageBox.Refresh();




        }

        private void ImageViewer_Load(object sender, EventArgs e)
        {
            
        }


    }
}
