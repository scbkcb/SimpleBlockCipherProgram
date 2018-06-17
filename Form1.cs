using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
         
       
namespace SimpleBlockCipherProgram
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //##########################################################################################################
        //---------------------Simple BLock Cipher------------------------------------------------------------------
        //##########################################################################################################
        //---------------------Khai báo các biến--------------------------------------------------------------------
        string FilePathIn = "";
        string FilePathOut = "";
        int NumRound = 2;
        long key= 1234;
        int[] k = new int[4];
        long soluong;
        long CheckQuad;
        long dem;
        int[,] State = new int[25000000,4];
        int[] sbox = new int[256] { 
	        0x63, 0x7c, 0x77, 0x7b, 0xf2, 0x6b, 0x6f, 0xc5, 0x30, 0x01, 0x67, 0x2b, 0xfe, 0xd7, 0xab, 0x76, //0
	        0xca, 0x82, 0xc9, 0x7d, 0xfa, 0x59, 0x47, 0xf0, 0xad, 0xd4, 0xa2, 0xaf, 0x9c, 0xa4, 0x72, 0xc0, //1
	        0xb7, 0xfd, 0x93, 0x26, 0x36, 0x3f, 0xf7, 0xcc, 0x34, 0xa5, 0xe5, 0xf1, 0x71, 0xd8, 0x31, 0x15, //2
	        0x04, 0xc7, 0x23, 0xc3, 0x18, 0x96, 0x05, 0x9a, 0x07, 0x12, 0x80, 0xe2, 0xeb, 0x27, 0xb2, 0x75, //3
	        0x09, 0x83, 0x2c, 0x1a, 0x1b, 0x6e, 0x5a, 0xa0, 0x52, 0x3b, 0xd6, 0xb3, 0x29, 0xe3, 0x2f, 0x84, //4
	        0x53, 0xd1, 0x00, 0xed, 0x20, 0xfc, 0xb1, 0x5b, 0x6a, 0xcb, 0xbe, 0x39, 0x4a, 0x4c, 0x58, 0xcf, //5
	        0xd0, 0xef, 0xaa, 0xfb, 0x43, 0x4d, 0x33, 0x85, 0x45, 0xf9, 0x02, 0x7f, 0x50, 0x3c, 0x9f, 0xa8, //6
	        0x51, 0xa3, 0x40, 0x8f, 0x92, 0x9d, 0x38, 0xf5, 0xbc, 0xb6, 0xda, 0x21, 0x10, 0xff, 0xf3, 0xd2, //7
	        0xcd, 0x0c, 0x13, 0xec, 0x5f, 0x97, 0x44, 0x17, 0xc4, 0xa7, 0x7e, 0x3d, 0x64, 0x5d, 0x19, 0x73, //8
	        0x60, 0x81, 0x4f, 0xdc, 0x22, 0x2a, 0x90, 0x88, 0x46, 0xee, 0xb8, 0x14, 0xde, 0x5e, 0x0b, 0xdb, //9
	        0xe0, 0x32, 0x3a, 0x0a, 0x49, 0x06, 0x24, 0x5c, 0xc2, 0xd3, 0xac, 0x62, 0x91, 0x95, 0xe4, 0x79, //A
	        0xe7, 0xc8, 0x37, 0x6d, 0x8d, 0xd5, 0x4e, 0xa9, 0x6c, 0x56, 0xf4, 0xea, 0x65, 0x7a, 0xae, 0x08, //B
	        0xba, 0x78, 0x25, 0x2e, 0x1c, 0xa6, 0xb4, 0xc6, 0xe8, 0xdd, 0x74, 0x1f, 0x4b, 0xbd, 0x8b, 0x8a, //C
	        0x70, 0x3e, 0xb5, 0x66, 0x48, 0x03, 0xf6, 0x0e, 0x61, 0x35, 0x57, 0xb9, 0x86, 0xc1, 0x1d, 0x9e, //D
	        0xe1, 0xf8, 0x98, 0x11, 0x69, 0xd9, 0x8e, 0x94, 0x9b, 0x1e, 0x87, 0xe9, 0xce, 0x55, 0x28, 0xdf, //E
	        0x8c, 0xa1, 0x89, 0x0d, 0xbf, 0xe6, 0x42, 0x68, 0x41, 0x99, 0x2d, 0x0f, 0xb0, 0x54, 0xbb, 0x16 }; //F};
        //useless S-box below
        int[] sbox2 = new int[256]{51,236,224,63,203,20,24,199,83,140,128,95, 171, 116,120, 167, 142, 81, 93, 130, 118, 169, 165, 122, 238, 49, 61, 226, 22, 201, 197, 26, 210, 13, 1, 222, 42, 245, 249, 38, 178, 109, 97, 190, 74, 149, 153, 70, 111, 176, 188, 99, 151, 72, 68, 155, 15, 208, 220, 3, 247, 40, 36, 251, 195, 28, 16, 207, 59, 228, 232, 55, 163, 124, 112, 175, 91, 132, 136, 87, 126, 161, 173, 114, 134, 89, 85, 138, 30, 193, 205, 18, 230, 57, 53, 234, 34, 253, 241, 46, 218, 5, 9, 214, 66, 157, 145, 78, 186, 101, 105, 182, 159, 64, 76, 147, 103, 184, 180, 107, 255, 32, 44, 243, 7, 216, 212, 11, 127, 160, 172, 115, 135, 88, 84, 139, 31, 192, 204, 19, 231, 56, 52, 235, 194, 29, 17, 206, 58, 229, 233, 54, 162, 125, 113, 174, 90, 133, 137, 86, 158, 65, 77, 146, 102, 185, 181, 106, 254, 33, 45, 242, 6, 217, 213, 10, 35, 252, 240, 47, 219, 4, 8, 215, 67, 156, 144, 79, 187, 100, 104, 183, 143, 80, 92, 131, 119, 168, 164, 123, 239, 48, 60, 227, 23, 200, 196, 27, 50, 237, 225, 62, 202, 21, 25, 198, 82, 141, 129, 94, 170, 117, 121, 166, 110, 177, 189, 98, 150, 73, 69, 154, 14, 209, 221, 2, 246, 41, 37, 250, 211, 12, 0, 223, 43, 244, 248, 39, 179, 108, 96, 191, 75, 148, 152, 71};
        int[] sboxRev = new int[256] { 
            0x52, 0x09, 0x6a, 0xd5, 0x30, 0x36, 0xa5, 0x38, 0xbf, 0x40, 0xa3, 0x9e, 0x81, 0xf3, 0xd7, 0xfb,
            0x7c, 0xe3, 0x39, 0x82, 0x9b, 0x2f, 0xff, 0x87, 0x34, 0x8e, 0x43, 0x44, 0xc4, 0xde, 0xe9, 0xcb,
            0x54, 0x7b, 0x94, 0x32, 0xa6, 0xc2, 0x23, 0x3d, 0xee, 0x4c, 0x95, 0x0b, 0x42, 0xfa, 0xc3, 0x4e,
            0x08, 0x2e, 0xa1, 0x66, 0x28, 0xd9, 0x24, 0xb2, 0x76, 0x5b, 0xa2, 0x49, 0x6d, 0x8b, 0xd1, 0x25,
            0x72, 0xf8, 0xf6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xd4, 0xa4, 0x5c, 0xcc, 0x5d, 0x65, 0xb6, 0x92,
            0x6c, 0x70, 0x48, 0x50, 0xfd, 0xed, 0xb9, 0xda, 0x5e, 0x15, 0x46, 0x57, 0xa7, 0x8d, 0x9d, 0x84,
            0x90, 0xd8, 0xab, 0x00, 0x8c, 0xbc, 0xd3, 0x0a, 0xf7, 0xe4, 0x58, 0x05, 0xb8, 0xb3, 0x45, 0x06,
            0xd0, 0x2c, 0x1e, 0x8f, 0xca, 0x3f, 0x0f, 0x02, 0xc1, 0xaf, 0xbd, 0x03, 0x01, 0x13, 0x8a, 0x6b,
            0x3a, 0x91, 0x11, 0x41, 0x4f, 0x67, 0xdc, 0xea, 0x97, 0xf2, 0xcf, 0xce, 0xf0, 0xb4, 0xe6, 0x73,
            0x96, 0xac, 0x74, 0x22, 0xe7, 0xad, 0x35, 0x85, 0xe2, 0xf9, 0x37, 0xe8, 0x1c, 0x75, 0xdf, 0x6e,
            0x47, 0xf1, 0x1a, 0x71, 0x1d, 0x29, 0xc5, 0x89, 0x6f, 0xb7, 0x62, 0x0e, 0xaa, 0x18, 0xbe, 0x1b,
            0xfc, 0x56, 0x3e, 0x4b, 0xc6, 0xd2, 0x79, 0x20, 0x9a, 0xdb, 0xc0, 0xfe, 0x78, 0xcd, 0x5a, 0xf4,
            0x1f, 0xdd, 0xa8, 0x33, 0x88, 0x07, 0xc7, 0x31, 0xb1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xec, 0x5f,
            0x60, 0x51, 0x7f, 0xa9, 0x19, 0xb5, 0x4a, 0x0d, 0x2d, 0xe5, 0x7a, 0x9f, 0x93, 0xc9, 0x9c, 0xef,
            0xa0, 0xe0, 0x3b, 0x4d, 0xae, 0x2a, 0xf5, 0xb0, 0xc8, 0xeb, 0xbb, 0x3c, 0x83, 0x53, 0x99, 0x61,
            0x17, 0x2b, 0x04, 0x7e, 0xba, 0x77, 0xd6, 0x26, 0xe1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0c, 0x7d };
        //----------------------------------------------------------------------------------------------------------
        //---------------------Function-----------------------------------------------------------------------------
        void readFile()
        {
            try
            {
                FileStream fs1 = new FileStream(FilePathIn, FileMode.Open);
                fs1.Seek(0, 0);
                soluong = fs1.Length;
                CheckQuad = soluong % 4;
                dem = soluong /4;
                for (int i = 0; i < dem; i++)
                    for (int j = 0; j < 4; j++)
                    {
                        State[i,j] = fs1.ReadByte();
                    }

                if (CheckQuad == 1)
                {
                    State[dem, 0] = 0;
                    State[dem, 1] = 0;
                    State[dem, 2] = 0;
                    State[dem, 3] = fs1.ReadByte();
                }
                if (CheckQuad == 2)
                {
                    State[dem, 0] = 0;
                    State[dem, 1] = 0;
                    State[dem, 2] = fs1.ReadByte();
                    State[dem, 3] = fs1.ReadByte();
                }
                if (CheckQuad == 3)
                {
                    State[dem, 0] = 0;
                    State[dem, 1] = fs1.ReadByte();
                    State[dem, 2] = fs1.ReadByte();
                    State[dem, 3] = fs1.ReadByte();
                }
                fs1.Close();
            }
            catch
            {
            }
        }

        void writeFile()
        {
            try
            {
                FileStream fs1 = new FileStream(FilePathOut, FileMode.Create);
                fs1.Seek(0, 0);
                for (int i = 0; i < dem; i++)
                    for (int j = 0; j < 4; j++) 
                        fs1.WriteByte((byte)State[i,j]);

                if (CheckQuad == 1)
                {
                    fs1.WriteByte((byte)State[dem,3]);
                }
                if (CheckQuad == 2)
                {
                    fs1.WriteByte((byte)State[dem, 2]);
                    fs1.WriteByte((byte)State[dem, 3]);
                } 
                if (CheckQuad == 3)
                {
                    fs1.WriteByte((byte)State[dem, 1]);
                    fs1.WriteByte((byte)State[dem, 2]);
                    fs1.WriteByte((byte)State[dem, 3]);
                }
                fs1.Close();
            }
            catch
            {
            }
        }
        long TextToLong(string tmp)
        {
            long ketqua = 0, x;
            for (int i = 0; i < tmp.Length; i++)
            {
                if ((tmp[i] >= '0') && (tmp[i] <= 9))
                    x = Convert.ToInt64(tmp[i]) - Convert.ToInt64('0');
                else x = Convert.ToInt64(tmp[i]);
                ketqua = (ketqua << 8) + x;
            }
            return ketqua;
        }

        int Cutter(long inp, int vitri) //tach sobit can bat dau tu vitri cua in, tinh tu phai sang trai
        {
            long tmp = inp >> (vitri * 8);
            tmp = tmp & 255;
            return Convert.ToInt32(tmp);
        }
        long Joiner(int in1, int in2, int in3, int in4) //ghep 8 bit o in2 vao duoi in1
        {
            long tmp = 0;
            tmp = in1 << 8;
            tmp |= in2;
            tmp = tmp << 8;
            tmp |= in3;
            tmp = tmp << 8;
            tmp |= in4;
            return tmp;
        }
        void KSF()
        {
            k[0] = Cutter(key, 3);
            k[1] = Cutter(key, 2);
            k[2] = Cutter(key, 1);
            k[3] = Cutter(key, 0);
        }

        void AddRoundKey(int vitri)
        {
            KSF();
            for (int i = 0; i < 4; i++)
                State[vitri, i] ^= k[i];
        }

        void SubBytes(int vitri)
        {
            for (int i = 0; i < 4; i++)
                State[vitri, i] = sbox[State[vitri, i]];
        }
        void InvSubBytes(int vitri)
        {
            for (int i = 0; i < 4; i++)
                State[vitri, i] = sboxRev[State[vitri, i]];
        }
        void ShiftRows(int vitri)
        {
            int tmp;
	        // dich vong i ve ben trai i byte	
	        tmp=State[vitri,2];
            State[vitri, 2] = State[vitri, 3];
            State[vitri, 3] = tmp;
        }
        
        void Encryptor()
        {
            for (int i=0;i<=dem;i++)  //cho từng khối
            {
                AddRoundKey(i); //Xor khóa lần đầu tiên
                for (long k=1;k < NumRound;k++) //n-1 vòng đầu
                {
                    SubBytes(i);
                    ShiftRows(i);
                    AddRoundKey(i);
                }
                SubBytes(i);  //Vòng cuối
                ShiftRows(i);
                AddRoundKey(i);
            }          
        }

        void Decryptor()
        {
            for (int i=0;i<=dem;i++)  //cho từng khối
            {
                AddRoundKey(i); //Xor khóa lần đầu tiên
                for (long k=1;k < NumRound;k++) //n-1 vòng đầu
                {
                    ShiftRows(i);
                    InvSubBytes(i);
                    AddRoundKey(i);
                }
                ShiftRows(i);
	            InvSubBytes(i);
	            AddRoundKey(i);
            }
        }

        //----------------------------------------------------------------------------------------------------------
        //---------------------WINFORM------------------------------------------------------------------------------
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void txtKey_Click(object sender, EventArgs e)
        {
        }

        private void FileInput_Click(object sender, EventArgs e)
        {
            if (FileInput.Text == "FILE CẦN MÃ HÓA/ GIẢI MÃ")
                FileInput.Text = "";
        }

        private void FileOutput_Click(object sender, EventArgs e)
        {
            if (FileOutput.Text == "VỊ TRÍ FILE KẾT QUẢ")
                FileOutput.Text = "";
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog oFile = new OpenFileDialog();
                oFile.Filter = "all files|*.*";
                oFile.RestoreDirectory = true;
                if (oFile.ShowDialog() == DialogResult.OK)
                {
                    FileInput.Text = oFile.FileName;
                    FilePathIn = oFile.FileName;
                }
            }
            catch
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog oFile = new OpenFileDialog();
                oFile.Filter = "all files|*.*";
                oFile.RestoreDirectory = true;
                if (oFile.ShowDialog() == DialogResult.OK)
                {
                    FileOutput.Text = oFile.FileName;
                    FilePathOut = oFile.FileName;
                }
            }
            catch
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txtKey.Text == "")
            {
                MessageBox.Show("YÊU CẦU NHẬP MẬT KHẨU !!!");
                return;
            }

            key = TextToLong(txtKey.Text);

            FilePathIn = FileInput.Text;
            if (FileOutput.Text == "" || FileOutput.Text == "VỊ TRÍ FILE KẾT QUẢ")
                FilePathOut = FileInput.Text;
            else
                FilePathOut = FileOutput.Text;
            readFile();
            Encryptor();
            writeFile();
            MessageBox.Show("MÃ HÓA THÀNH CÔNG !!!");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (txtKey.Text == "")
            {
                MessageBox.Show("YÊU CẦU NHẬP MẬT KHẨU !!!");
                return;
            }
            key = TextToLong(txtKey.Text);

            FilePathIn = FileInput.Text;
            if (FileOutput.Text == "" || FileOutput.Text == "VỊ TRÍ FILE KẾT QUẢ")
                FilePathOut = FileInput.Text;
            else
                FilePathOut = FileOutput.Text;

            readFile();
            Decryptor();
            writeFile();
            MessageBox.Show("GIẢI MÃ THÀNH CÔNG !!!");
        }

        //##########################################################################################################
        
        //##########################################################################################################
        //-----------------------------Differential Analysis--------------------------------------------------------
        //##########################################################################################################
        //---------------------Khai báo các biến--------------------------------------------------------------------
        int[,] CalFreq1 = new int[256, 256];
        int[,] CalFreq = new int[256, 256];		//To Calculate Frequency of DiffDiffrential Characteristic
        int delta0, delta1;						//Diffrential Characteristic found
        long numPairs = 64;					//Define number of known pairs
        long[] knownP0, knownP1, knownC0, knownC1, goodP0, goodP1, goodC0, goodC1;		//if C0 xor delta1 = C1
        int numGpair = 0; //so cap dung

        //----------------------------------------------------------------------------------------------------------
        //---------------------Function-----------------------------------------------------------------------------
        long LongRandom(long min, long max, Random rand)
        {
            byte[] buf = new byte[8];
            rand.NextBytes(buf);
            long longRand = BitConverter.ToInt64(buf, 0);
            return (Math.Abs(longRand % (max - min)) + min);
        }
        private void StrToBox(string myStr)
        {
            DiffTextShow.AppendText(myStr);
        }
        long AutoEncrypt(long input, long rkey)
        {
            for (int i = 0; i < 4; i++)
                State[0, 3 - i] = Cutter(Convert.ToInt64(input), i);
            AddRoundKey(0); //Xor khóa lần đầu tiên
            for (long k = 1; k < NumRound; k++) //n-1 vòng đầu
            {
                SubBytes(0);
                ShiftRows(0);
                AddRoundKey(0);
            }
            SubBytes(0);  //Vòng cuối
            ShiftRows(0);
            AddRoundKey(0);
            long output = Joiner(State[0, 0], State[0, 1], State[0, 2], State[0, 3]);
            return output;
        }

        int EncryptState(int _state)
        {
            long P = Joiner(0, 0, _state, 0);
            long C = AutoEncrypt(P, key);
            int vitriState = 1;
            if (NumRound % 2 == 1) vitriState = 0;
            return Cutter(C, vitriState);
        }
        void findDiffs()
        {
            StrToBox("----------------------------------------------------\n");
            StrToBox("BẮT ĐẦU TẠO BẢNG ĐẶC TÍNH VI SAI: \n");
            CalFreq = new int[256, 256];
            int c, d; //tmp delta
            int tmp = 1;
            for (int P0_2 = 0; P0_2 < 256; P0_2++)
                for (int P1_2 = 0; P1_2 < 256; P1_2++)
                {
                    //CalFreq[P0_2 ^ P1_2, (sbox[sbox[(P0_2)]] ^ sbox[sbox[(P1_2)]])]++;
                    CalFreq[P0_2 ^ P1_2, (EncryptState(P0_2) ^ EncryptState(P1_2))]++;  //calculate frequency of delta0 -> delta1 
                }
                 /*   
            for (c = 0; c < 256; c++)
            {
                StrToBox("\n");
                for (d = 0; d < 256; d++)
                    StrToBox(CalFreq[c, d] + " ");
            }
            */
            for (c = 0; c < 256; c++)
                for (d = 0; d < 256; d++)
                    if (c == 0 && d == 0) { }
                    else
                        if (tmp < CalFreq[c, d])
                        {
                            tmp = CalFreq[c,d];
                        };
            //for (int dempair = 2; dempair <= NumRound; dempair++) 
            //    numPairs *= 64;
            numPairs = 4096;

            StrToBox("\n----------------------------------------------------\n");
            StrToBox("HIỂN THỊ CÁC ĐẶC TÍNH CÓ KHẢ NĂNG CAO NHẤT: \n");
            for (c = 0; c < 256; c++)
                for (d = 0; d < 256; d++)
                    if (CalFreq[c,d] == tmp)
                    {
                        StrToBox("    " + CalFreq[c, d] + " : " + c.ToString("X") + "  -->  " + d.ToString("X") + "\n");
                        delta0 = c;
                        delta1 = d;
                    };
        }

        void findGoodPair(int indiff, int outdiff)
        {
            StrToBox("\n----------------------------------------------------\n");
            StrToBox("TÌM KIẾM CẶP ĐÚNG DỰA TRÊN ĐẶC TÍNH VI SAI:" + indiff.ToString("X") + "  -->  " + outdiff.ToString("X"));
            StrToBox("\n----------------------------------------------------\n");
            //StrToBox("DUYỆT CÁC CẶP ĐƯỢC TẠO DỰA TRÊN KHÁC BIỆT ĐẦU VÀO:\n");
            long c;
            knownP0 = new long[numPairs];
            knownP1 = new long[numPairs];
            knownC0 = new long[numPairs];
            knownC1 = new long[numPairs];
            goodP0 = new long[numPairs];
            goodC0 = new long[numPairs];
            goodP1 = new long[numPairs];
            goodC1 = new long[numPairs];
            for (c = 0; c < numPairs; c++)
            {
                knownP0[c] = LongRandom(0x01, 0xffffffff, new Random());
                if (c > 0)
                    while (knownP0[c] == knownP0[c-1])
                        knownP0[c] = LongRandom(0x01, 0xffffffff, new Random());
    
                int P0_2 = Cutter(knownP0[c], 1);
                int P1_2 = P0_2 ^ indiff;
                long tmp = 0;
                tmp = (tmp | P1_2) << 8;
                knownP1[c] = (knownP0[c] & 0xffff00ff) | tmp;
                knownC0[c] = AutoEncrypt(knownP0[c], key);
                knownC1[c] = AutoEncrypt(knownP1[c], key);
                //StrToBox("(P0 = " + knownP0[c].ToString("X") + ", P1 = " + knownP1[c].ToString("X") + ") 
                    //--> (C0 = " + knownC0[c].ToString("X") + ", C1 = " + knownC1[c].ToString("X") + ")\n");

                int C0_2 = Cutter(knownC0[c], 1);
                int C1_2 = Cutter(knownC1[c], 1);
                if ((C0_2 ^ C1_2) == outdiff)                               //Does the ciphertext pair fit the intacteristic?
                {
                    goodC0[numGpair] = knownC0[c];
                    goodC1[numGpair] = knownC1[c];
                    goodP0[numGpair] = knownP0[c];
                    goodP1[numGpair] = knownP1[c];
                    StrToBox("(P0 = " + goodP0[numGpair].ToString("X") + ", P1 = " + goodP1[numGpair].ToString("X") 
                        + ") --> (C0 = " + goodC0[numGpair].ToString("X") + ", C1 = " + goodC1[numGpair].ToString("X") + ")\n");
                    numGpair++;
                }
            }
            StrToBox("\n----------------------------------------------------\n");    
            StrToBox("CÓ " + numGpair + " CẶP ĐÚNG  !!!\n");
        }

        int testKey(long testK)
        {
            long c;
            int crap = 0;
            for (c = 0; c < numPairs; c++)
            {
                if ((AutoEncrypt(goodP0[c], key) != goodC0[c]) || (AutoEncrypt(goodP1[c], key) != goodC1[c]))
                {
                    crap = 1;
                    break;
                }
            }

            if (crap == 0)
                return 1;
            else
                return 0;
        }
        
        void crack()
        {
            long testK = 0;
            StrToBox("\nTÌM KIẾM VÉT CẠN TRÊN KHÔNG GIAN KHÓA ĐÃ GIẢM:\n");
            //Giải mã một phần
            long tmp1, tmp2;
            int[] count0,count1;
            int tmp, tmpp;

            //--------------------------------------------------
            count1 = new int[256];
            //--Trai--
            for (int tk1 = 0; tk1 < 0xff; tk1++)
            {
                for (int j = 0; j < numGpair; j++)
                {
                    long tk = Joiner(0, tk1, 0, 0);
                    tmp1 = sboxRev[Cutter(tk ^ goodC0[j], 2)];
                    tmp2 = sboxRev[Cutter(tk ^ goodC1[j], 2)];
                    if (delta1 == (tmp1 ^ tmp2)) count1[tk1]++;
                }
            }
            tmp = 1;
            tmpp = 0;
            for (int tk1 = 0; tk1 < 0xff; tk1++)

                if (tmp < count1[tk1])
                {
                    tmp = count1[tk1];
                    tmpp = tk1;
                }
            //StrToBox(tmpp.ToString("X"));
            //--Phai--
            for (int tk0 = 0; tk0 < 0xff; tk0++)
            {
                for (int j = 0; j < numGpair; j++)
                {
                    long tk = Joiner(0, tk0, 0, 0);
                    tmp1 = sboxRev[Cutter(tk ^ goodC0[j], 2)];
                    tmp2 = sboxRev[Cutter(tk ^ goodC1[j], 2)];
                    if (delta1 == (tmp1 ^ tmp2)) count1[tk0]++;
                }
            }
            tmp = 1;
            tmpp = 0;
            for (int tk0 = 0; tk0 < 0xff; tk0++)

                if (tmp < count1[tk0])
                {
                    tmp = count1[tk0];
                    tmpp = tk0;
                }
            //StrToBox(tmpp.ToString("X"));
            
            if (testKey(key) == 1)        
            {                        
                StrToBox("       " + key.ToString("X") + " --> KEY! \n");                       
                return;                   
            }              
            else       
                StrToBox("       " + key.ToString("X") + "\n");
        }
        //----------------------------------------------------------------------------------------------------------
        //---------------------WINFORM------------------------------------------------------------------------------
        private void button6_Click(object sender, EventArgs e)
        {
            DiffTextShow.Text = "";
            findDiffs();
            StrToBox("\n\n\n" + numPairs);
            Console.Beep(1000, 1000);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            DiffTextShow.Text = "";
            findGoodPair(delta0,delta1);
            Console.Beep(1000, 1000);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DiffTextShow.Text = "";
            crack();
            //StrToBox((0x2f ^ 0x5f).ToString("X") + "  " + (sbox[0x2f] ^ sbox[0x5f]).ToString("X") + "  "
            //    + (sbox[sbox[0x2f]] ^ sbox[sbox[0x5f]]).ToString("X") + "  " + (EncryptState(0x2f) ^ EncryptState(0x5f)).ToString("X"));
            Console.Beep(1000,1000);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            key = 1234;
            DiffTextShow.Text = "";
            findDiffs();
            findGoodPair(delta0,delta1);
            crack();
            Console.Beep(1000, 1000);
        } 
        //##########################################################################################################
    }
}
