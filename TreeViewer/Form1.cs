using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TreeViewer {
    public partial class Form1 : Form {
        class Node {
            public Node Left;
            public Node Right;
            public object data;
        }

        class Tree {
            public Node root;

            int Height(Node node) {
                if (node == null) return 0;
                int height = 1;

                if (node.Left != null) height += Height(node.Left);
                if (node.Right != null) height = Math.Max(height, Height(node.Right));

                return height;
            }

            public int Height() {
                if (root == null) {
                    return 0;
                }

                return Height(root);
            }

            void DrawTree(Graphics gi, Node node, int a, int b, int h, int fromx, int fromy) {
                if (node == null) return;

                Console.WriteLine(a + ", " + b);

                int sign = a < 0 ? -1 : 1;

                int x = 700 + a * 20;
                int y = 200 + b * 100;

                Console.WriteLine(x + ", " + y);

                gi.FillRectangle(Brushes.Black, x, y, 5, 5);
                gi.DrawLine(Pens.Black, fromx, fromy, x+2, y+2);

                gi.DrawString("(" + a + ", " + b + ")", SystemFonts.DefaultFont, Brushes.Black, x + 5, y);

                int add = (int)(Math.Pow(2, h - b)*0.5);

                DrawTree(gi, node.Left, a- add, b+1, h, x+2, y+2);
                DrawTree(gi, node.Right, a+ add, b+1, h, x+2, y+2);
            }

            public void DrawTree(Graphics gi) {
                gi.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                int height = Height() - 1;

                gi.FillRectangle(Brushes.Black, 700 , 200, 5, 5);

                DrawTree(gi, root.Left, -(int)(Math.Pow(2, height) * 0.5), 1, height, 700+2, 200+2);
                DrawTree(gi, root.Right, (int)(Math.Pow(2, height) * 0.5), 1, height, 700+2, 200+2);
            }
        }

        Tree tree = new Tree();

        public Form1() {
            InitializeComponent();
            DoubleBuffered = true;
            ResizeRedraw = true;
            BackColor = Color.White;

            tree.root = new Node();
            tree.root.Left = new Node();
            tree.root.Right = new Node();


            tree.root.Left.Left = new Node();
            tree.root.Left.Right = new Node();

            tree.root.Right.Left = new Node();
            tree.root.Right.Right = new Node();


            tree.root.Left.Left.Left = new Node();
            tree.root.Left.Left.Right = new Node();
            tree.root.Left.Right.Left = new Node();
            tree.root.Left.Right.Right = new Node();

            tree.root.Right.Left.Left = new Node();
            tree.root.Right.Left.Right = new Node();
            tree.root.Right.Right.Left = new Node();
            tree.root.Right.Right.Right = new Node();


            tree.root.Left.Left.Left.Left = new Node();
            tree.root.Left.Left.Left.Right = new Node();

            tree.root.Left.Left.Right.Left = new Node();
            tree.root.Left.Left.Right.Right = new Node();

            tree.root.Left.Right.Left.Left = new Node();
            tree.root.Left.Right.Left.Right = new Node();

            tree.root.Left.Right.Right.Left = new Node();
            tree.root.Left.Right.Right.Right = new Node();

            tree.root.Right.Left.Left.Left = new Node();
            tree.root.Right.Left.Left.Right = new Node();

            tree.root.Right.Left.Right.Left = new Node();
            tree.root.Right.Left.Right.Right = new Node();

            tree.root.Right.Right.Left.Left = new Node();
            tree.root.Right.Right.Left.Right = new Node();

            tree.root.Right.Right.Right.Left = new Node();
            tree.root.Right.Right.Right.Right = new Node();


            tree.root.Left.Left.Left.Left.Left = new Node();
            tree.root.Left.Left.Left.Left.Right = new Node();
            tree.root.Left.Left.Left.Right.Left = new Node();
            tree.root.Left.Left.Left.Right.Right = new Node();

            tree.root.Left.Left.Right.Left.Left = new Node();
            tree.root.Left.Left.Right.Left.Right = new Node();
            tree.root.Left.Left.Right.Right.Left = new Node();
            tree.root.Left.Left.Right.Right.Right = new Node();

            tree.root.Left.Right.Left.Left.Left = new Node();
            tree.root.Left.Right.Left.Left.Right = new Node();
            tree.root.Left.Right.Left.Right.Left = new Node();
            tree.root.Left.Right.Left.Right.Right = new Node();

            tree.root.Left.Right.Right.Left.Left = new Node();
            tree.root.Left.Right.Right.Left.Right = new Node();
            tree.root.Left.Right.Right.Right.Left = new Node();
            tree.root.Left.Right.Right.Right.Right = new Node();

            tree.root.Right.Left.Left.Left.Left = new Node();
            tree.root.Right.Left.Left.Left.Right = new Node();
            tree.root.Right.Left.Left.Right.Left = new Node();
            tree.root.Right.Left.Left.Right.Right = new Node();

            tree.root.Right.Left.Right.Left.Left = new Node();
            tree.root.Right.Left.Right.Left.Right = new Node();
            tree.root.Right.Left.Right.Right.Left = new Node();
            tree.root.Right.Left.Right.Right.Right = new Node();

            tree.root.Right.Right.Left.Left.Left = new Node();
            tree.root.Right.Right.Left.Left.Right = new Node();
            tree.root.Right.Right.Left.Right.Left = new Node();
            tree.root.Right.Right.Left.Right.Right = new Node();

            tree.root.Right.Right.Right.Left.Left = new Node();
            tree.root.Right.Right.Right.Left.Right = new Node();
            tree.root.Right.Right.Right.Right.Left = new Node();
            tree.root.Right.Right.Right.Right.Right = new Node();
        }

        private void Form1_Paint(object sender, PaintEventArgs e) {
            /* int height = tree.Height();
            int width = (int)Math.Pow(2, height);

            int boxWidth = 20;
            int boxHeight = 20; */

            tree.DrawTree(e.Graphics);
        }
    }
}
