using System.Collections;
using System.Collections.Generic;

public class TetrisData
{
    public int[,] board = new int[10, 24];
    public int[] next = new int[100];
    public bool holdEnable = true;
    public int hold = 0;
    public int current = 0;

    public int minoX = 0;
    public int minoY = 0;
    public int minoR = 0;

    public int prevSrs = 0;
    public bool prevSpin = false;
    public bool btb = false;
    public int ren = 0;

    public int deleteLine = 0;
    public int sendLine = 0;
    public int TsmCount = 0;
    public int TsmdCount = 0;
    public int TssCount = 0;
    public int TsdCount = 0;
    public int TstCount = 0;
    public int TetrisCount = 0;
    public int PcCount = 0;

    public TetrisData(TetrisData src)
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                this.board[x, y] = src.board[x, y];
            }
        }

        for (int i = 0; i < 100; i++)
        {
            next[i] = src.next[i];
        }

        holdEnable = src.holdEnable;
        hold = src.hold;
        current = src.current;
        minoX = src.minoX;
        minoY = src.minoY;
        minoR = src.minoR;
        prevSrs = src.prevSrs;
        prevSpin = src.prevSpin;
        btb = src.btb;
        ren = src.ren;
        deleteLine = src.deleteLine;
        sendLine = src.sendLine;
        TsmCount = src.TsmCount;
        TsmdCount = src.TsmdCount;
        TssCount = src.TssCount;
        TsdCount = src.TsdCount;
        TstCount = src.TstCount;
        TetrisCount = src.TetrisCount;
        PcCount = src.PcCount;
    }

    public TetrisData(int[,] b, int[] n)
    {
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                this.board[x, y] = b[x, y];
            }
        }

        current = n[0];
        hold = n[1];
        for (int i = 0; i < 100; i++)
        {
            if ((i + 2) >= n.Length)
            {
                break;
            }

            next[i] = n[i + 2];
        }
    }

    public bool newGame(bool firstStep = false)
    {
        /// もう次がない場合は終了
        if ((next[0] == 0) && (hold == 0))
        {
            return false;
        }

        /// とりあえずカレントをNEXTから取り出す。
        holdEnable = true;
        if ((firstStep == false) || (current == 0))
        {
            getNext();
        }

        /// カレントの位置を設定します。
        POINT currentPoint = GetMinoPisitionStart(board, current);
        if (currentPoint == null)
        {
            minoX = 3;
            minoY = 9;
            minoR = 0;
            return false;
        }

        minoX = currentPoint.x;
        minoY = currentPoint.y;
        minoR = 0;
        prevSrs = 0;
        prevSpin = false;

        /// NEXTが存在しなくてHoldもない場合はHoldボタンは使えません。
        if ((next[0] == 0) && (hold == 0))
        {
            holdEnable = false;
        }
        if (hold == 8)
        {
            holdEnable = false;
        }

        return true;
    }

    public bool finishDrop()
    {
        /// T-Spin判定を行います。
        int tSpinType = TetrisData.CheckTSpin(board, current, new POINT() { x = minoX, y = minoY }, minoR, prevSpin, prevSrs);

        /// ラインの消去を行います。
        int delete = TetrisData.CorrectBoard(board, false);

        /// 火力数を計算します。
        int send = TetrisData.CheckSendline(board, delete, tSpinType, ren, btb);

        /// btbを設定します。
        if (delete > 0)
        {
            if (tSpinType != 0)
            {
                btb = true;
            }
            else if (delete >= 4)
            {
                btb = true;
            }
            else
            {
                btb = false;
            }
        }

        /// renを設定します。
        if (delete > 0)
        {
            ren++;
        }
        else
        {
            ren = 0;
        }

        /// 結果データの設定を行います。
        if (send >= 10)
        {
            PcCount++;
        }
        if ((tSpinType == 2) && (delete == 1))
        {
            TsmCount++;
        }
        if ((tSpinType == 2) && (delete == 2))
        {
            TsmdCount++;
        }
        if ((tSpinType == 1) && (delete == 1))
        {
            TssCount++;
        }
        if ((tSpinType == 1) && (delete == 2))
        {
            TsdCount++;
        }
        if ((tSpinType == 1) && (delete == 3))
        {
            TstCount++;
        }
        if (delete == 4)
        {
            TetrisCount++;
        }

        /// 
        bool result = newGame();
        return result;
    }

    /// ミノの形状を考慮した開始座標を取得します
    public POINT GetMinoPisitionStart(int[,] board, int current)
    {
        POINT currrentPoint = new POINT() { x = 3, y = 8 };

        /// 20段目でぶつかっていなかったら、出現成功
        POINT[] CellPoints = GetRotateMinoPoints(current, currrentPoint, 0);
        if (CheckCollision(board, CellPoints) == false)
        {
            return currrentPoint;
        }

        /// 21段目の出現をチェックします。
        currrentPoint.y++;

        /// 21段目でぶつかっていなかったら、出現成功
        CellPoints = GetRotateMinoPoints(current, currrentPoint, 0);
        if (CheckCollision(board, CellPoints) == false)
        {
            return currrentPoint;
        }

        /// 死亡
        return null;
    }

    public bool Hold()
    {
        if (holdEnable == false)
        {
            return true;
        }

        /// ホールドとカレントを入れ替えます。
        int temp = current;
        current = hold;
        hold = temp;

        /// カレントが空の場合はNEXTから取り出します。
        if (current == 0)
        {
            getNext();
        }

        /// カレントの位置を設定します。
        POINT currentPoint = GetMinoPisitionStart(board, current);
        if (currentPoint == null)
        {
            return false;
        }

        minoX = currentPoint.x;
        minoY = currentPoint.y;
        minoR = 0;

        holdEnable = false;
        return true;
    }

    public void getNext()
    {
        current = next[0];
        for (int i = 1; i < next.Length; i++)
        {
            next[i - 1] = next[i];
        }
        next[next.Length - 1] = 0;

        /// それでもカレントが存在しなければホールドからデータを取り出します。
        if (current == 0)
        {
            Hold();
        }
    }

    public class POINT
    {
        public int x = 0;
        public int y = 0;

        public POINT() { }

        public POINT(POINT src)
        {
            x = src.x;
            y = src.y;
        }
    }

    /// ミノのデフォルト形状です。ポイントで表します。
    private static POINT[,] MINO_FORM_S = new POINT[4, 4] {
        { new POINT() { x = 1, y =  0}, new POINT() { x = 2, y =  0}, new POINT() { x = 0, y = -1}, new POINT() { x = 1, y = -1} },
        { new POINT() { x = 1, y =  0}, new POINT() { x = 1, y = -1}, new POINT() { x = 2, y = -1}, new POINT() { x = 2, y = -2} },
        { new POINT() { x = 1, y = -1}, new POINT() { x = 2, y = -1}, new POINT() { x = 0, y = -2}, new POINT() { x = 1, y = -2} },
        { new POINT() { x = 0, y =  0}, new POINT() { x = 0, y = -1}, new POINT() { x = 1, y = -1}, new POINT() { x = 1, y = -2} },
    };
    private static POINT[,] MINO_FORM_Z = new POINT[4, 4] {
        { new POINT() { x = 0, y =  0}, new POINT() { x = 1, y =  0}, new POINT() { x = 1, y = -1}, new POINT() { x = 2, y = -1} },
        { new POINT() { x = 2, y =  0}, new POINT() { x = 1, y = -1}, new POINT() { x = 2, y = -1}, new POINT() { x = 1, y = -2} },
        { new POINT() { x = 0, y = -1}, new POINT() { x = 1, y = -1}, new POINT() { x = 1, y = -2}, new POINT() { x = 2, y = -2} },
        { new POINT() { x = 1, y =  0}, new POINT() { x = 0, y = -1}, new POINT() { x = 1, y = -1}, new POINT() { x = 0, y = -2} },
    };                                                     
    private static POINT[,] MINO_FORM_T = new POINT[4, 4] {  
        { new POINT() { x = 1, y =  0}, new POINT() { x = 0, y = -1}, new POINT() { x = 1, y = -1}, new POINT() { x = 2, y = -1} },
        { new POINT() { x = 1, y =  0}, new POINT() { x = 1, y = -1}, new POINT() { x = 2, y = -1}, new POINT() { x = 1, y = -2} },
        { new POINT() { x = 0, y = -1}, new POINT() { x = 1, y = -1}, new POINT() { x = 2, y = -1}, new POINT() { x = 1, y = -2} },
        { new POINT() { x = 1, y =  0}, new POINT() { x = 0, y = -1}, new POINT() { x = 1, y = -1}, new POINT() { x = 1, y = -2} },
    };                                                                         
    private static POINT[,] MINO_FORM_L = new POINT[4, 4] {      
        { new POINT() { x = 2, y =  0}, new POINT() { x = 0, y = -1}, new POINT() { x = 1, y = -1}, new POINT() { x = 2, y = -1} },
        { new POINT() { x = 1, y =  0}, new POINT() { x = 1, y = -1}, new POINT() { x = 1, y = -2}, new POINT() { x = 2, y = -2} },
        { new POINT() { x = 0, y = -1}, new POINT() { x = 1, y = -1}, new POINT() { x = 2, y = -1}, new POINT() { x = 0, y = -2} },
        { new POINT() { x = 0, y =  0}, new POINT() { x = 1, y =  0}, new POINT() { x = 1, y = -1}, new POINT() { x = 1, y = -2} },
    };                                                             
    private static POINT[,] MINO_FORM_J = new POINT[4, 4] {               
        { new POINT() { x = 0, y =  0}, new POINT() { x = 0, y = -1}, new POINT() { x = 1, y = -1}, new POINT() { x = 2, y = -1} },
        { new POINT() { x = 1, y =  0}, new POINT() { x = 2, y =  0}, new POINT() { x = 1, y = -1}, new POINT() { x = 1, y = -2} },
        { new POINT() { x = 0, y = -1}, new POINT() { x = 1, y = -1}, new POINT() { x = 2, y = -1}, new POINT() { x = 2, y = -2} },
        { new POINT() { x = 1, y =  0}, new POINT() { x = 1, y = -1}, new POINT() { x = 0, y = -2}, new POINT() { x = 1, y = -2} },
    };                                                     
    private static POINT[,] MINO_FORM_I = new POINT[4, 4] { 
        { new POINT() { x = 0, y = -1}, new POINT() { x = 1, y = -1}, new POINT() { x = 2, y = -1}, new POINT() { x = 3, y = -1} },
        { new POINT() { x = 2, y =  0}, new POINT() { x = 2, y = -1}, new POINT() { x = 2, y = -2}, new POINT() { x = 2, y = -3} },
        { new POINT() { x = 0, y = -2}, new POINT() { x = 1, y = -2}, new POINT() { x = 2, y = -2}, new POINT() { x = 3, y = -2} },
        { new POINT() { x = 1, y =  0}, new POINT() { x = 1, y = -1}, new POINT() { x = 1, y = -2}, new POINT() { x = 1, y = -3} },
    };                                                      
    private static POINT[,] MINO_FORM_O = new POINT[4, 4] { 
        { new POINT() { x = 1, y =  0}, new POINT() { x = 2, y =  0}, new POINT() { x = 1, y = -1}, new POINT() { x = 2, y = -1} },
        { new POINT() { x = 1, y =  0}, new POINT() { x = 2, y =  0}, new POINT() { x = 1, y = -1}, new POINT() { x = 2, y = -1} },
        { new POINT() { x = 1, y =  0}, new POINT() { x = 2, y =  0}, new POINT() { x = 1, y = -1}, new POINT() { x = 2, y = -1} },
        { new POINT() { x = 1, y =  0}, new POINT() { x = 2, y =  0}, new POINT() { x = 1, y = -1}, new POINT() { x = 2, y = -1} },
    };

    /// Aボタン右回転時のオフセットデータです。
    private static POINT[,] SRS_OTHER_A = new POINT[4, 5] {
        { new POINT() { x = 0, y =  0}, new POINT() { x = -1, y = 0}, new POINT() { x = -1, y = -1}, new POINT() { x =  0, y = +2}, new POINT() { x = -1, y = +2} }, // D->A
        { new POINT() { x = 0, y =  0}, new POINT() { x = -1, y = 0}, new POINT() { x = -1, y = +1}, new POINT() { x =  0, y = -2}, new POINT() { x = -1, y = -2} }, // A->B
        { new POINT() { x = 0, y =  0}, new POINT() { x = +1, y = 0}, new POINT() { x = +1, y = -1}, new POINT() { x =  0, y = +2}, new POINT() { x = +1, y = +2} }, // B->C
        { new POINT() { x = 0, y =  0}, new POINT() { x = +1, y = 0}, new POINT() { x = +1, y = +1}, new POINT() { x =  0, y = -2}, new POINT() { x = +1, y = -2} }, // C->D
    };                                                       
                                                             
    /// Bボタン左回転時のオフセットデータです。              
    private static POINT[,] SRS_OTHER_B = new POINT[4, 5] {  
        { new POINT() { x = 0, y =  0}, new POINT() { x = +1, y = 0}, new POINT() { x = +1, y = -1}, new POINT() { x =  0, y = +2}, new POINT() { x = +1, y = +2} }, // B->A
        { new POINT() { x = 0, y =  0}, new POINT() { x = -1, y = 0}, new POINT() { x = -1, y = +1}, new POINT() { x =  0, y = -2}, new POINT() { x = -1, y = -2} }, // C->B
        { new POINT() { x = 0, y =  0}, new POINT() { x = -1, y = 0}, new POINT() { x = -1, y = -1}, new POINT() { x =  0, y = +2}, new POINT() { x = -1, y = +2} }, // D->C
        { new POINT() { x = 0, y =  0}, new POINT() { x = +1, y = 0}, new POINT() { x = +1, y = +1}, new POINT() { x =  0, y = -2}, new POINT() { x = +1, y = -2} }, // A->D
    };
                                                           
    /// Aボタン右回転時のオフセットデータです。            
    private static POINT[,] SRS_IO_A = new POINT[4, 5] {   
        { new POINT() { x = 0, y =  0}, new POINT() { x = +1, y = 0}, new POINT() { x = -2, y =  0}, new POINT() { x = +1, y = -2}, new POINT() { x = -2, y = +1} }, // D->A
        { new POINT() { x = 0, y =  0}, new POINT() { x = -2, y = 0}, new POINT() { x = +1, y =  0}, new POINT() { x = -2, y = -1}, new POINT() { x = +1, y = +2} }, // A->B
        { new POINT() { x = 0, y =  0}, new POINT() { x = -1, y = 0}, new POINT() { x = +2, y =  0}, new POINT() { x = -1, y = +2}, new POINT() { x = +2, y = -1} }, // B->C
        { new POINT() { x = 0, y =  0}, new POINT() { x = +2, y = 0}, new POINT() { x = -1, y =  0}, new POINT() { x = +2, y = +1}, new POINT() { x = -1, y = -2} }, // C->D
    };                                                     
                                                           
    /// Bボタン左回転時のオフセットデータです。            
    private static POINT[,] SRS_IO_B = new POINT[4, 5] {   
        { new POINT() { x = 0, y =  0}, new POINT() { x = +2, y = 0}, new POINT() { x = -1, y =  0}, new POINT() { x = +2, y = +1}, new POINT() { x = -1, y = -2} }, // B->A
        { new POINT() { x = 0, y =  0}, new POINT() { x = +1, y = 0}, new POINT() { x = -2, y =  0}, new POINT() { x = +1, y = -2}, new POINT() { x = -2, y = +1} }, // C->B
        { new POINT() { x = 0, y =  0}, new POINT() { x = -2, y = 0}, new POINT() { x = +1, y =  0}, new POINT() { x = -2, y = -1}, new POINT() { x = +1, y = +2} }, // D->C
        { new POINT() { x = 0, y =  0}, new POINT() { x = -1, y = 0}, new POINT() { x = +2, y =  0}, new POINT() { x = -1, y = +2}, new POINT() { x = +2, y = -1} }, // A->D
    };

    /// 回転させたミノの位置を返します。
    public static POINT[] GetRotateMinoPoints(int current, POINT currentPoint, int r)
    {
        POINT[] minoCells = new POINT[4]
        {
            new POINT(),
            new POINT(),
            new POINT(),
            new POINT()
        };

        switch (current)
        {
            case 5:
                for (int i = 0; i < 4; i++)
                {
                    minoCells[i].x = currentPoint.x + MINO_FORM_S[r, i].x;
                    minoCells[i].y = currentPoint.y + MINO_FORM_S[r, i].y;
                }
                break;
            case 7:
                for (int i = 0; i < 4; i++)
                {
                    minoCells[i].x = currentPoint.x + MINO_FORM_Z[r, i].x;
                    minoCells[i].y = currentPoint.y + MINO_FORM_Z[r, i].y;
                }
                break;
            case 6:
                for (int i = 0; i < 4; i++)
                {
                    minoCells[i].x = currentPoint.x + MINO_FORM_T[r, i].x;
                    minoCells[i].y = currentPoint.y + MINO_FORM_T[r, i].y;
                }
                break;
            case 3:
                for (int i = 0; i < 4; i++)
                {
                    minoCells[i].x = currentPoint.x + MINO_FORM_L[r, i].x;
                    minoCells[i].y = currentPoint.y + MINO_FORM_L[r, i].y;
                }
                break;
            case 2:
                for (int i = 0; i < 4; i++)
                {
                    minoCells[i].x = currentPoint.x + MINO_FORM_J[r, i].x;
                    minoCells[i].y = currentPoint.y + MINO_FORM_J[r, i].y;
                }
                break;
            case 1:
                for (int i = 0; i < 4; i++)
                {
                    minoCells[i].x = currentPoint.x + MINO_FORM_I[r, i].x;
                    minoCells[i].y = currentPoint.y + MINO_FORM_I[r, i].y;
                }
                break;
            case 4:
                for (int i = 0; i < 4; i++)
                {
                    minoCells[i].x = currentPoint.x + MINO_FORM_O[r, i].x;
                    minoCells[i].y = currentPoint.y + MINO_FORM_O[r, i].y;
                }
                break;
        }

        return minoCells;
    }

    /// ミノとフィールドが衝突しているかどうかを返します。
    public static bool CheckCollision(int[,] board, POINT[] cellPoints)
    {
        for (int i = 0; i < 4; i++)
        {
            if (cellPoints[i].x > (10 - 1)) return true;
            if (cellPoints[i].x < 0) return true;
            if (cellPoints[i].y < 0) return true;
            if (cellPoints[i].y > (24 - 1)) return true;

            if (board[cellPoints[i].x, cellPoints[i].y] != 0)
            {
                return true;
            }
        }

        return false;
    }


    /// スーパーローテーションシステムを実装します。返り値はSRSパターンです
    public static (int, POINT) GetSuperRotateMinoPoint(int[,] board, int current, POINT currentPoint, int r, int nextR)
    {
        // Oミノは回転しません。
        if (current == 4)
        {
            return (0, new POINT(currentPoint));
        }

        /// 回転先の位置を決定しておきます。
        r += nextR;
        if (r >= 4)
        {
            r = 0;
        }
        if (r < 0)
        {
            r = 3;
        }

        /// 使用する軸ずれテーブルを取得します。
        POINT[,] srsType = null;
        if (current == 1) 
        {
            /// +方向の回転、右回転の処理です。
            if (nextR > 0)
            {
                srsType = SRS_IO_A;
            }

            /// -方向の回転、左回転の処理です。
            if (nextR < 0)
            {
                srsType = SRS_IO_B;
            }
        }
        else
        {
            /// +方向の回転、右回転の処理です。
            if (nextR > 0)
            {
                srsType = SRS_OTHER_A;
            }

            /// -方向の回転、左回転の処理です。
            if (nextR < 0)
            {
                srsType = SRS_OTHER_B;
            }
        }

        POINT destPoint = new POINT(currentPoint);

        //// どことも衝突していなければ回転成立として値を返します。
        {
            POINT[] mino_point = GetRotateMinoPoints(current, destPoint, r);
            if (CheckCollision(board, mino_point) == false)
            {
                return (0, new POINT(destPoint));
            }
        }

        //// 軸補正を行います。
        for (int i = 1; i < 5; i++)
        {
            //// 衝突していた場合は、次の場所に移動してチェックを行います。
            ///// まず移動を戻します。
            destPoint.x -= srsType[r, i - 1].x;
            destPoint.y -= srsType[r, i - 1].y;

            ///// 移動させます。
            destPoint.x += srsType[r, i].x;
            destPoint.y += srsType[r, i].y;

            //// どことも衝突していなければ回転成立として値を返します。
            {
                POINT[] mino_point = GetRotateMinoPoints(current, destPoint, r);
                if (CheckCollision(board, mino_point) == false)
                {
                    return (i, new POINT(destPoint));
                }
            }
        }

        /// 回転できませんでした。
        return (-1, new POINT(currentPoint));
    }

    private static POINT FallCollosion(int[,] board, int current, POINT currentPoint, int r)
    {
        POINT destPoint = new POINT(currentPoint);
        POINT[] CellPoints = GetRotateMinoPoints(current, currentPoint, r);

        while (true)
        {
            // y座標を減少
            for (int i = 0; i < 4; i++)
            {
                CellPoints[i].y--;
            }
            destPoint.y--;

            // ぶつかるかチェック
            bool collision = false;

            for (int i = 0; i < 4; i++)
            {
                if (CellPoints[i].y < 0 || (board[CellPoints[i].x, CellPoints[i].y] != 0))
                {
                    collision = true;
                    break;
                }
            }

            // ぶつかった場合、座標を一つ戻してループを終了
            if (collision)
            {
                destPoint.y++;
                break;
            }
        }

        return destPoint;
    }

    /// TSpinかどうかの判定を行います。
    /// 1はTSpin、2はTSpinMiniです
    private static int CheckTSpin(int[,] board, int current, POINT currentPoint, int r, bool spin, int srs)
    {
        /// TミノでなければTスピンできません
        /// スピンしてなければTスピンできません
        if ((spin == false) || current != 6)
        {
            return 0;
        }

        /// 4隅のセル情報を取得します。
        /// 向きにかかわらず凸として左上、右上、右下、左下を設定します。
        /// 凸の後ろも取りたいので、4方向取っておきます。
        bool[] cell = new bool[4];

        if ((currentPoint.x >= 0) && (currentPoint.x < 10) && (currentPoint.y >= 0) && (currentPoint.y < 24))
        {
            if (board[currentPoint.x, currentPoint.y] != 0)
            {
                cell[0] = true;
            }
        }
        else
        {
            cell[0] = true;
        }
        if ((currentPoint.x >= 0) && (currentPoint.x < (10 - 2)) && (currentPoint.y >= 0) && (currentPoint.y < 24))
        {
            if (board[currentPoint.x + 2, currentPoint.y] != 0)
            {
                cell[1] = true;
            }
        }
        else
        {
            cell[1] = true;
        }
        if ((currentPoint.x >= 0) && (currentPoint.x < (10 - 2)) && (currentPoint.y >= 2) && (currentPoint.y < 24))
        {
            if (board[currentPoint.x + 2, currentPoint.y - 2] != 0)
            {
                cell[2] = true;
            }
        }
        else
        {
            cell[2] = true;
        }
        if ((currentPoint.x >= 0) && (currentPoint.x < 10) && (currentPoint.y >= 2) && (currentPoint.y < 24))
        {
            if (board[currentPoint.x, currentPoint.y - 2] != 0)
            {
                cell[3] = true;
            }
        }
        else
        {
            cell[3] = true;
        }

        /// 回転に合わせてセル情報を回転させます。
        for (int i = r; i > 0; i--)
        {
            bool tempCell = cell[3];
            cell[3] = cell[0];
            cell[0] = cell[1];
            cell[1] = cell[2];
            cell[2] = tempCell;
        }

        /// 最低3隅が埋まっていなければTSpinになりません。
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            if (cell[i] == true) count++;
        }
        if (count < 3) return 0;

        /// 3隅のうち、0,1が埋まっていなければTSpinMiniです。
        if (cell[0] != cell[1])
        {
            if (srs != 4) return 2;
        }

        return 1;
    }

    /// ↓への移動を行います。
    public static POINT ManualDown(int[,] board, int current, POINT currentPoint, int r)
    {
        POINT movePoint = new POINT(currentPoint);
        movePoint.y--;
        POINT[] dest_points = GetRotateMinoPoints(current, movePoint, r);
        bool result = CheckCollision(board, dest_points);

        if (result == false)
        {
            return movePoint;
        }

        movePoint.y++;
        return movePoint;
    }

    /// ←への移動を行います。
    public static POINT ManualLeft(int[,] board, int current, POINT currentPoint, int r)
    {
        POINT movePoint = new POINT(currentPoint);
        movePoint.x--;
        POINT[] dest_points = GetRotateMinoPoints(current, movePoint, r);
        bool result = CheckCollision(board, dest_points);

        if (result == false)
        {
            return movePoint;
        }

        movePoint.x++;
        return movePoint;
    }

    /// →への移動を行います。
    public static POINT ManualRight(int[,] board, int current, POINT currentPoint, int r)
    {
        POINT movePoint = new POINT(currentPoint);
        movePoint.x++;
        POINT[] dest_points = GetRotateMinoPoints(current, movePoint, r);
        bool result = CheckCollision(board, dest_points);

        if (result == false)
        {
            return movePoint;
        }

        movePoint.x--;
        return movePoint;
    }

    /// 右回転を行います。
    public static (int, POINT, int) ManualA(int[,] board, int current, POINT currentPoint, int r)
    {
        if (current == 4)
        {
            return (0, currentPoint, r);
        }

        (int result, POINT destPoint) = GetSuperRotateMinoPoint(board, current, currentPoint, r, 1);

        /// ミノを回転させます。
        if (result >= 0)
        {
            r += 1;
            if (r >= 4)
            {
                r = 0;
            }
        }

        return (result, destPoint, r);
    }

    /// 左回転を行います。
    public static (int, POINT, int) ManualB(int[,] board, int current, POINT currentPoint, int r)
    {
        if (current == 4)
        {
            return (0, currentPoint, r);
        }

        (int result, POINT destPoint) = GetSuperRotateMinoPoint(board, current, currentPoint, r, -1);

        /// ミノを回転させます。
        if (result >= 0)
        {
            r -= 1;
            if (r < 0)
            {
                r = 3;
            }
        }

        return (result, destPoint, r);
    }

    /// 一番↓への移動を行います。
    public static (int, POINT) ManualAllDown(int[,] board, int current, POINT currentPoint, int r)
    {
        POINT movePoint = new POINT(currentPoint);
        bool result = false;
        int dropCount = 0;
        while (result == false)
        {
            movePoint.y--;
            POINT[] destPoint = GetRotateMinoPoints(current, movePoint, r);
            result = CheckCollision(board, destPoint);

            if (result == true)
            {
                movePoint.y++;
                return (dropCount, movePoint);
            }

            dropCount++;
        }

        return (dropCount, movePoint);
    }

    /// その場所にミノを置きます。
    public static void ManualPut(int[,] board, int current, POINT currentPoint, int r)
    {
        POINT movePoint = new POINT(currentPoint);
        POINT[] destPoint = GetRotateMinoPoints(current, movePoint, r);

        for (int i = 0; i < 4; i++)
        {
            board[destPoint[i].x, destPoint[i].y] = current;
        }
    }

    /// フィールドを正規化し、ライン消去を行います。
    /// 0-19ライン目までしか消去しません。
    public static int CorrectBoard(int[,] board, bool clear = true)
    {
        int delete_count = 0;

        for (int y = 0; y < 20; y++)
        {
            bool fill = true;
            for (int x = 0; x < 10; x++)
            {
                if (board[x, y] == 0)
                {
                    fill = false;
                    break;
                }
            }

            if (fill == true)
            {
                delete_count++;
                if (clear == true)
                {
                    for (int y2 = y; y2 < (24 - 1); y2++)
                    {
                        for (int x = 0; x < 10; x++)
                        {
                            board[x, y2] = board[x, y2 + 1];
                        }
                    }
                    y--;
                }
            }
        }

        return delete_count;
    }

    /// 送信ライン数の計算を行います。
    public static int CheckSendline(int[,] board, int deleteLine, int tSpinType, int ren, bool btb)
    {
        /// 何も消していなければスコア0です。
        if (deleteLine == 0)
        {
            return 0;
        }

        /// パフェ状態
        int cellCount = 0;
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 5; y++)
            {
                if ((board[x, y] != 0) && (board[x, y] != 9))
                {
                    cellCount++;
                    break;
                }
            }
            if (cellCount > 0) { break; }
        }
        if (cellCount == 0)
        {
            return 10;
        }

        /// 消したラインにより火力が変動します。
        int attack = 0;
        if (deleteLine == 1)
        {
            if (tSpinType == 1)
            {
                attack = 2;
                if (btb == true) attack++;
            }
            else if (tSpinType == 2)
            {
                if (btb == true) attack++;
            }
        }
        if (deleteLine == 2)
        {
            if (tSpinType == 1)
            {
                attack = 4;
                if (btb == true) attack++;
            }
            else if (tSpinType == 2)
            {
                attack = 1;
                if (btb == true) attack++;
            }
            else attack = 1;
        }
        if (deleteLine == 3)
        {
            if (tSpinType == 1)
            {
                attack = 6;
                if (btb == true) attack++;
            }
            else attack = 2;
        }
        if (deleteLine == 4)
        {
            attack = 4;
            if (btb == true) attack++;
        }

        /// REN火力を加算します。
        switch (ren)
        {
            case 0:
            case 1:
            case 2:
            case 3: attack += 0; break;
            case 4:
            case 5: attack += 1; break;
            case 6:
            case 7: attack += 2; break;
            case 8:
            case 9: attack += 3; break;
            case 10:
            case 11: attack += 4; break;
            case 12:
            case 13: attack += 5; break;
            default: attack += 6; break;
        }

        return attack;
    }
}