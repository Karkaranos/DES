using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Linq;

public class InputManager : MonoBehaviour
{
    private string plaintext;
    private string key;
    private string rawKeyBinary;
    private int[] permutatedBinaryKey;
    private int[,] keys;

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
    #endregion
    public void StartEncryption()
    {
        key = keyField.text;
        GenerateKeys();
    }

    private void GenerateKeys()
    {
        permutatedBinaryKey = new int[56];
        keys = new int[16,8];
        if (key.Length > 8)
            key = key.Substring(0, 8);

        Debug.Log(key);
        rawKeyBinary = "";


        for(int i=7; i>=0; i--)
        {
            rawKeyBinary = CharAsBinary(key.Substring(i, 1)) + rawKeyBinary;
        }
        Debug.Log(rawKeyBinary);

        int[] removedParity = new int[56];
        for(int i=0, j=0; i<64; i++)
        {
            if((i+1)%8 == 0)
            {
                continue;
            }
            removedParity[j] = rawKeyBinary.ElementAt(i);
            j++;
        }

        KeyPC1(removedParity);
    }


    private string CharAsBinary(string c)
    {
        string res = "";
        int value = ASCIIVals[c];
        for(int i=7; i>=0; i--)
        {
            if(value >= (int)Mathf.Pow(2,i))
            {
                value -= (int)Mathf.Pow(2, i);
                res += "1";
            }
            else
            {
                res += "0";
            }
        }
        return res;
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




}
