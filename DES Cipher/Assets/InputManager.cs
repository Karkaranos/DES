using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Linq;
using System;
using UnityEngine.InputSystem.DualShock.LowLevel;

public class InputManager : MonoBehaviour
{
    private string plaintext;
    private string key;
    private int[] rawKeyBinary;
    private int[] permutatedBinaryKey;
    [SerializeField] private int[][] keys = new int[16][];

    [SerializeField] private TMP_Text keyField;
    [SerializeField] private TMP_Text plaintextField;

    #region Dictionaries (numeric and Alpha for now)
    Dictionary<string, int> ASCIIVals = new Dictionary<string, int>
    {
        { "!", 33 },
        {"#", 35},
        {"$", 36 },
        {"%", 37 },
        {"&", 38 },
        {"0", 48 },
        {"1", 49},
        {"2", 50 },
        {"3" , 51},
        {"4", 52 },
        {"5", 53 },
        {"6", 54 },
        {"7", 55 },
        {"8", 56 },
        {"9", 57 },
        {"A", 65},
        {"B", 66},
        {"C", 67 },
        {"D", 68},
        {"E", 69},
        {"F", 70},
        {"G", 71},
        {"H", 72},
        {"I", 73},
        {"J", 74},
        {"K", 75},
        {"L", 76},
        {"M", 77},
        {"N", 78},
        {"O", 79},
        {"P", 80},
        {"Q", 81},
        {"R", 82},
        {"S", 83},
        {"T", 84},
        {"U", 85},
        {"V", 86},
        {"W", 87},
        {"X", 88},
        {"Y", 89},
        {"Z", 90},
        {"a", 97},
        {"b", 98},
        {"c", 99},
        {"d", 100},
        {"e", 101},
        {"f", 102},
        {"g", 103},
        {"h", 104},
        {"i", 105},
        {"j", 106},
        {"k", 107},
        {"l", 108},
        {"m", 109},
        {"n", 110},
        {"o", 111},
        {"p", 112},
        {"q", 113},
        {"r", 114},
        {"s", 115},
        {"t", 116},
        {"u", 117},
        {"v", 118},
        {"w", 119},
        {"x", 120},
        {"y", 121},
        {"z", 122}

    };
    Dictionary<int, int> PC1 = new Dictionary<int, int>
    {
        {0,8},
        {1,16},
        {2,24},
        {3,56},
        {4,52},
        {5,44},
        {6,36},
        {7,7},
        {8,15},
        {9,23},
        {10,55},
        {11,51},
        {12,43},
        {13,35},
        {14,6},
        {15,14},
        {16,22},
        {17,54},
        {18,50},
        {19,42},
        {20,34},
        {21,5},
        {22,13},
        {23,21},
        {24,53},
        {25,49},
        {26,41},
        {27,33},
        {28,4},
        {29,12},
        {30,20},
        {31,28},
        {32,48},
        {33,40},
        {34,32},
        {35,3},
        {36,11},
        {37,19},
        {38,27},
        {39,47},
        {40,39},
        {41,31},
        {42,2},
        {43,10},
        {44,18},
        {45,26},
        {46,46},
        {47,38},
        {48,30},
        {49,1},
        {50,9},
        {51,17},
        {52,25},
        {53,45},
        {54,37},
        {55,29}
    };
    Dictionary<int, int> PC2 = new Dictionary<int, int>
    {
        {0,5},
        {1,24},
        {2,7},
        {3,16},
        {4,6},
        {5,10},
        {6,20},
        {7,18},
        {8, -1},
        {9,12},
        {10,3},
        {11,15},
        {12,23},
        {13,1},
        {14,9},
        {15,19},
        {16,2},
        {17,-1},
        {18,14},
        {19,22},
        {20,11},
        {21,-1},
        {22,13},
        {23,4},
        {24,-1},
        {25,17},
        {26,21},
        {27,8},
        {28,47},
        {29,31},
        {30,27},
        {31,48},
        {32,35},
        {33,41},
        {34,-1},
        {35,46},
        {36,28},
        {37,-1},
        {38,39},
        {39,32},
        {40,25},
        {41,44},
        {42,-1},
        {43,37},
        {44,34},
        {45,43},
        {46,29},
        {47,36},
        {48,38},
        {49,45},
        {50,33},
        {51,26},
        {52,42},
        {53,-1},
        {54,30},
        {55,40}
    };
    Dictionary<int, int> IP = new Dictionary<int, int>
    {
        {0,40},
        {1,8},
        {2,48},
        {3,16},
        {4,56},
        {5,24},
        {6,64},
        {7,32},
        {8,39},
        {9,7},
        {10,47},
        {11,15},
        {12,55},
        {13,23},
        {14,63},
        {15,31},
        {16,38},
        {17,6},
        {18,46},
        {19,14},
        {20,54},
        {21,22},
        {22,62},
        {23,30},
        {24,37},
        {25,5},
        {26,45},
        {27,13},
        {28,53},
        {29,21},
        {30,61},
        {31,29},
        {32,36},
        {33,4},
        {34,44},
        {35,12},
        {36,52},
        {37,20},
        {38,60},
        {39,28},
        {40,35},
        {41,3},
        {42,43},
        {43,11},
        {44,51},
        {45,19},
        {46,59},
        {47,27},
        {48,34},
        {49,2},
        {50,42},
        {51,10},
        {52,50},
        {53,18},
        {54,58},
        {55,26},
        {56,33},
        {57,1},
        {58,41},
        {59,9},
        {60,49},
        {61,17},
        {62,57},
        {63,25}
    };
    Dictionary<int, int> IP1 = new Dictionary<int, int>
    {
        {0,58},
        {1,50},
        {2,42},
        {3,34},
        {4,26},
        {5,18},
        {6,10},
        {7,2},
        {8,60},
        {9,52},
        {10,44},
        {11,36},
        {12,28},
        {13,20},
        {14,12},
        {15,4},
        {16,62},
        {17,54},
        {18,46},
        {19,38},
        {20,30},
        {21,22},
        {22,14},
        {23,6},
        {24,64},
        {25,56},
        {26,48},
        {27,40},
        {28,32},
        {29,24},
        {30,16},
        {31,8},
        {32,57},
        {33,49},
        {34,41},
        {35,33},
        {36,25},
        {37,17},
        {38,9},
        {39,1},
        {40,59},
        {41,51},
        {42,43},
        {43,35},
        {44,27},
        {45,19},
        {46,11},
        {47,3},
        {48,61},
        {49,53},
        {50,45},
        {51,37},
        {52,29},
        {53,21},
        {54,13},
        {55,5},
        {56,63},
        {57,55},
        {58,47},
        {59,39},
        {60,31},
        {61,23},
        {62,15},
        {63,7}
    };
    Dictionary<int, int> E = new Dictionary<int, int>
    {
        {0,248},
        {1,3},
        {2,4},
        {3,0507},
        {4,0608},
        {5,9},
        {6,10},
        {7,1113},
        {8,1214},
        {9,15},
        {10,16},
        {11,1719},
        {12,1820},
        {13,21},
        {14,22},
        {15,2325},
        {16,2426},
        {17,27},
        {18,28},
        {19,2931},
        {20,3032},
        {21,33},
        {22,34},
        {23,3537},
        {24,3638},
        {25,39},
        {26,40},
        {27,4143},
        {28,4244},
        {29,45},
        {30,46},
        {31,147}
    };
    #endregion

    #region S Boxes
    private int[,] S1 = new int[,]
    {
        {14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7},
        {0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8},
        {4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0},
        {15, 12, 8, 2, 4, 9, 1, 7, 5,  11, 3, 14, 10, 0, 6, 13}
    };

    private int[,] S2 = new int[,]
    {
        {15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10},
        {3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5},
        {0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15},
        {13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9}
    };

    private int[,] S3 = new int[,]
    {
        {10, 0, 9, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10},
        {13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1},
        {13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7},
        {1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12}
    };

    private int[,] S4 = new int[,]
    {
        {7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15},
        {13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9},
        {10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4},
        {3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14}
    };

    private int[,] S5 = new int[,]
    {
        {2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9},
        {14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6},
        {4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14},
        {11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3}
    };

    private int[,] S6 = new int[,]
    {
        {12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11},
        {10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8},
        {9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6},
        {4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13}
    };

    private int[,] S7 = new int[,]
    {
        {4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1},
        {13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6},
        {1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2},
        {6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12}
    };

    private int[,] S8 = new int[,]
    {
        {13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7},
        {1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2},
        {7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8},
        {2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11}
    };
    #endregion

    public void StartEncryption()
    {
        key = keyField.text;
        GenerateKeys();
    }

    private void GenerateKeys()
    {
        permutatedBinaryKey = new int[56];
        keys = new int[16][];
        if (key.Length > 8)
            key = key.Substring(0, 8);

        Debug.Log(key);
        rawKeyBinary = new int[64];


        for (int i = 7; i >=0; i--)
        {
            var result = CharAsBinary(key.Substring(i, 1));
            for (int j = 0; j < 8; j++)
            {
                rawKeyBinary[(8 * i) + j] = result[j];
            }
        }
        //Debug.Log(rawKeyBinary);

        int[] removedParity = new int[56];
        for (int i = 0, j = 0; i < 64; i++)
        {
            if ((i + 1) % 8 == 0)
            {
                continue;
            }
            removedParity[j] = rawKeyBinary.ElementAt(i);
            j++;
        }

        KeyPC1(removedParity);

        int[] CStart = new int[28];
        int[] DStart = new int[28];

        for (int i = 0; i < 28; i++)
        {
            CStart[i] = permutatedBinaryKey[i];
        }
        for (int i = 0, j = 28; i < 28 && j < 56; i++, j++)
        {
            DStart[i] = permutatedBinaryKey[j];
        }

        int[] CEnd = new int[28];
        int[] DEnd = new int[28];


        Array.Copy(CStart, CEnd, 28);
        Array.Copy(DStart, DEnd, 28);

        for (int i = 1; i <= 2; i++)
        {
            if (i == 1 || i == 2 || i == 9 || i==16)
            {
                CEnd = LeftShiftN(CStart, 2);
                DEnd = LeftShiftN(DStart, 2);
            }
            else
            {
                CEnd = LeftShiftN(CStart, 1);
                DEnd = LeftShiftN(DStart, 1);
            }

            CStart = CEnd;
            DStart = DEnd;

            int[] cocenatedVals = new int[56];
            cocenatedVals = CocenateArrays(CEnd, DEnd);

            keys[i] = cocenatedVals;

            

        }
    }


    private int[] CharAsBinary(string c)
    {
        int[] res = new int[8];
        int value = ASCIIVals[c];
        for(int i=7; i>=0; i--)
        {
            if(value >= (int)Mathf.Pow(2,i))
            {
                value -= (int)Mathf.Pow(2, i);
                res[i] = 1;
            }
            else
            {
                res[i] = 0;
            }
        }
        return res;
    }

    private int[] LeftShiftN(int[] start, int shift)
    {
        int[] result = new int[start.Length];
        int startingIndex = shift;
        int endingIndex = start.Length - shift;

        if(shift ==1)
        {
            result[result.Length - 1] = start[0];
        }
        else if(shift == 2)
        {
            result[result.Length - 2] = start[0];
            result[result.Length - 1] = start[1];
        }
            for (int i = startingIndex; i < endingIndex; i++)
            {
                result[(i-shift)] = start[i];
            }

        return result;
    }


    private int[] CocenateArrays(int[] c, int[] d)
    {
        int[] result = new int[c.Length + d.Length];

        for(int i=0; i<c.Length; i++)
        {
            result[i] = c[i];
        }

        for(int i=0, j=c.Length; j < result.Length && i<d.Length; i++, j++)
        {
            result[j] = d[i];
        }

        return result;
    }

    private void KeyPC1(int[] vals)
    {
        if(vals.Length < 56)
        {
            Debug.LogError("Error! Missing values for Key Permutation 1!");
            return;
        }

        for(int i=0; i<56; i++)
        {
            permutatedBinaryKey[PC1[i]-1] = vals[i];
        }
    }

    private int[] KeyPC2(int[] vals)
    {
        int[] result = new int[48];
        if(vals.Length < 56)
        {
            Debug.LogWarning("Error! Missing values for Key Permutation 2!");

        }
        for (int i = 0; i < 56; i++)
        {
            int res = PC2[i];
            if(res < 0)
            {
                continue;
            }
            result[res - 1] = vals[i];
        }
        return result;
    }





}
