/*
* JRC-PaStMe © European Union, 2018
* 
* Licensed under the EUPL, Version 1.2 or – as soon they
will be approved by the European Commission - subsequent
versions of the EUPL (the "Licence");
* You may not use this work except in compliance with the Licence.
* You may obtain a copy of the Licence at:
* 
* https://joinup.ec.europa.eu/software/page/eupl
* 
* Unless required by applicable law or agreed to in
writing, software distributed under the Licence is
distributed on an "AS IS" basis,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
either express or implied.
* See the Licence for the specific language governing
permissions and limitations under the Licence.
*/


using System;
using System.Collections.Generic; //Useful for List<string>
using System.Text.RegularExpressions; //Useful for regual expressions in BF_strengthMetric (BRUTE FORCE)
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Data.Matlab; // Needed for class MatlabReader
//using MathNet.Numerics.LinearAlgebra.Double;
using DotNetDoctor.csmatio; // Needed fpr class MatFileReader
using DotNetDoctor.csmatio.io;
using DotNetDoctor.csmatio.types;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JRC_PastMe // All the developed routines will belong to this namespace
{
    public static class PasswordStrength // Name of the static class enclosing all the static methods.
                                         // Static, therefore functions can be used without any instance of the class.
    {

        static string[] ReturnListOfReadLinesInTxtFile_StringArray(string TxtFilePath)
        // This function reads every line in a txt file (provided as input variable) and then all the lines are
        // returned in the output string array.
        {
            string[] fileContent = System.IO.File.ReadAllLines(TxtFilePath);
            return fileContent;
        }


        static List<string> ReturnListOfReadLinesInTxtFile_StringList(string TxtFilePath)
        // This function reads every line in a txt file (provided as input variable) and then all the lines are
        // returned in the output List<string>.
        // NOTE 1: Maybe this function can be removed in the future, if never used.
        // NOTE 2: The function ReturnListOfReadLinesInTxtFile_StringArray() is exploited.
        {
            List<string> linesList = new List<string>();
            string[] fileContent = ReturnListOfReadLinesInTxtFile_StringArray(TxtFilePath);
            linesList.AddRange(fileContent);
            return linesList;
        }




        public static MLDouble LoadTensorFromMatlabFile_Double(string MatlabFileName,
                                                               string MatTensorName_Double)
        {
            // Let's use CSmatIO library
            var mfr = new MatFileReader(MatlabFileName);
            MLArray mlArrayRetrieved = mfr.GetMLArray(MatTensorName_Double); // We need to find out of which type mlArrayRetrieved really is
                                                                             // (as MLArray is only a base class) and you cast into the real type.
                                                                             // Depending on the real type there are different methods/properties
                                                                             // that represent the data.

            MLDouble mlTensor = (mfr.Content[MatTensorName_Double] as MLDouble); // Get a reference to the read matlab variable
                                                                                 // Now MLDouble can be null or the desired tensor of double values
            return mlTensor;
        }

        public static MLDouble LoadScalarFromMatlabFile_Double(string MatlabFileName,
                                                               string MatScalarName_Double)
        {
            // Let's use CSmatIO library
            var mfr = new MatFileReader(MatlabFileName);
            MLArray mlArrayRetrieved = mfr.GetMLArray(MatScalarName_Double); // We need to find out of which type mlArrayRetrieved really is
                                                                             // (as MLArray is only a base class) and you cast into the real type.
                                                                             // Depending on the real type there are different methods/properties
                                                                             // that represent the data.

            MLDouble mlScalar = (mfr.Content[MatScalarName_Double] as MLDouble); // Get a reference to the read matlab variable
                                                                                 // Now MLDouble can be null or the desired double value
            return mlScalar;
        }


        public static MLChar LoadStringFromMatlabFile_Char(string MatlabFileName,
                                                       string MatScalarName_String)
        {
            // Let's use CSmatIO library
            var mfr = new MatFileReader(MatlabFileName);
            MLArray mlArrayRetrieved = mfr.GetMLArray(MatScalarName_String); // We need to find out of which type mlArrayRetrieved really is
                                                                             // (as MLArray is only a base class) and you cast into the real type.
                                                                             // Depending on the real type there are different methods/properties
                                                                             // that represent the data.

            MLChar mlScalar = (mfr.Content[MatScalarName_String] as MLChar); // Get a reference to the read matlab variable
                                                                             // Now MLChar can be null or the desired information

            return mlScalar;
        }




        public static double ReturnTensorValue_Double(MLDouble Tensor_Double,
                                                      int RowIndex,   // 0-based
                                                      int ColIndex,   // 0-based
                                                      int SliceIndex) // 0-based
        {
            // CSmatIO library is used
            int SingleIndex = RowIndex +
                           ColIndex * Tensor_Double.Dimensions[0] +
                           SliceIndex * Tensor_Double.Dimensions[0] * Tensor_Double.Dimensions[1];
            return Tensor_Double.Get(SingleIndex);
        }


        public static double ReturnTheMinimumNonZeroValueInATensor_Double(MLDouble Tensor_Double)
        {
            double minNonZeroValue = Double.PositiveInfinity; // +Inf at the beginning
            int NumberOfRowsToScan = Tensor_Double.Dimensions[0];
            int NumberOfColumnsToScan = Tensor_Double.Dimensions[1];

            if (Tensor_Double.NDimensions == 3)
            {   // Really it's a tensor (with 3 dimensions)
                int NumberOfSlicesToScan = Tensor_Double.Dimensions[2];

                for (int i = 0; i < NumberOfRowsToScan; i++)
                {
                    for (int j = 0; j < NumberOfColumnsToScan; j++)
                    {
                        for (int z = 0; z < NumberOfSlicesToScan; z++)
                        {
                            double CurrentScannedElement = ReturnTensorValue_Double(Tensor_Double, i, j, z);
                            if ((CurrentScannedElement > 0) && (CurrentScannedElement < minNonZeroValue))
                            {
                                minNonZeroValue = CurrentScannedElement;
                            }
                        }
                    }
                }
                return minNonZeroValue;
            }
            else
            {   // Really it's a matrix (with 2 dimensions) rather than a tensor
                for (int i = 0; i < NumberOfRowsToScan; i++)
                {
                    for (int j = 0; j < NumberOfColumnsToScan; j++)
                    {
                        double CurrentScannedElement = Tensor_Double.Get(i + j * Tensor_Double.Dimensions[0]);
                        if ((CurrentScannedElement > 0) && (CurrentScannedElement < minNonZeroValue))
                        {
                            minNonZeroValue = CurrentScannedElement;
                        }
                    }
                }
                return minNonZeroValue;

            }
        }

        public static double ReturnTheMinimumNonZeroValueInASliceOfATensor_Double(MLDouble Tensor_Double, int SliceIndex)
        {
            double minNonZeroValue = Double.PositiveInfinity; // +Inf at the beginning
            int NumberOfRowsToScan = Tensor_Double.Dimensions[0];
            int NumberOfColumnsToScan = Tensor_Double.Dimensions[1];

            for (int i = 0; i < NumberOfRowsToScan; i++)
            {
                for (int j = 0; j < NumberOfColumnsToScan; j++)
                {
                    double CurrentScannedElement = ReturnTensorValue_Double(Tensor_Double, i, j, SliceIndex);
                    if ((CurrentScannedElement > 0) && (CurrentScannedElement < minNonZeroValue))
                    {
                        minNonZeroValue = CurrentScannedElement;
                    }
                }
            }
            if (minNonZeroValue == Double.PositiveInfinity)
            {
                //there was no value in the tensor to scan therefore we set up a low value as a bonus
                minNonZeroValue = Math.Pow(10,-9);
            }
            return minNonZeroValue;
        }

        public static double ReturnTheMinimumNonZeroValueInAMatrix_Double(MLDouble Matrix_Double)
        {
            double minNonZeroValue = Double.PositiveInfinity; // +Inf at the beginning
            int NumberOfRowsToScan = Matrix_Double.Dimensions[0];
            int NumberOfColumnsToScan = Matrix_Double.Dimensions[1];

            for (int i = 0; i < NumberOfRowsToScan; i++)
            {
                for (int j = 0; j < NumberOfColumnsToScan; j++)
                {
                    double CurrentScannedElement = Matrix_Double.Get(i, j);
                    if ((CurrentScannedElement > 0) && (CurrentScannedElement < minNonZeroValue))
                    {
                        minNonZeroValue = CurrentScannedElement;
                    }
                }
            }
            return minNonZeroValue;
        }

        public static double ReturnTheMinimumNonZeroValueInAVector_Double(MLDouble Vector_Double)
        {
            double minNonZeroValue = Double.PositiveInfinity; // +Inf at the beginning
            int NumberOfElementsToScan = Vector_Double.Dimensions[0];

            for (int i = 0; i < NumberOfElementsToScan; i++)
            {
                double CurrentScannedElement = Vector_Double.Get(i);
                if ((CurrentScannedElement > 0) && (CurrentScannedElement < minNonZeroValue))
                {
                    minNonZeroValue = CurrentScannedElement;
                }
            }
            return minNonZeroValue;
        }

        public static double ReturnTheMinimumNonZeroValueInARowOfAMatrix_Double(Matrix<double> InputMatrix, int RowToScan)
        {
            double MinOfNonZeroValues = Double.PositiveInfinity; // +Inf at the beginning

            int NumberOfColumnsToScan = InputMatrix.ColumnCount;
            for (int j = 0; j < NumberOfColumnsToScan; j++)
            {
                double CurrentScannedElement = InputMatrix[RowToScan, j];
                if (CurrentScannedElement != 0)
                {
                    if (CurrentScannedElement < MinOfNonZeroValues)
                    {
                        MinOfNonZeroValues = CurrentScannedElement;
                    }
                }

            }
            return MinOfNonZeroValues;

        }

        public static int[] ConvertStringInUnicode1KIntCodePoints(string InputString)
        {
            int[] Unicode1KCodePoints = new int[InputString.Length];
            int i;
            for (i=0; i < InputString.Length; i++)
            {
                Unicode1KCodePoints[i] = (int)InputString[i];
            }
            return Unicode1KCodePoints;
        }


        //////////////////////////////////////////////
        // BEGIN OF ROUTINES ABOUT LIST_strengthMetric
        //////////////////////////////////////////////

        static int LevenshteinDistance(string s, string t)

        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            for (int i = 0; i <= n; i++) { d[i, 0] = i; }

            for (int j = 0; j <= m; j++) { d[0, j] = j; }

            for (int j = 1; j <= m; j++)
            {
                for (int i = 1; i <= n; i++)
                {
                    if (s[i - 1] == t[j - 1])
                        d[i, j] = d[i - 1, j - 1];  //no operation
                    else
                        d[i, j] = Math.Min(Math.Min(
                            d[i - 1, j] + 1,    //a deletion
                            d[i, j - 1] + 1),   //an insertion
                            d[i - 1, j - 1] + 1 //a substitution
                            );
                }
            }
            return d[n, m];
        }




        static double LIST_strengthMetric_FromListAsArrayOfString(string Password, string[] ListOfPasswords)
        // This function tests the strength of the input Password against a list of provided banned passwords,
        // according to the LIST criterion.
        // Two double values can be returned: 10 (strong password) or 0 (weak password).
        // Weak password means that the distance (between the input Password and at least one entry in the banned
        // passwords list) is equal or below 1.
        {
            double Slist = 10.0;
            int NumberOfPasswordsInList = ListOfPasswords.Length;
            if (NumberOfPasswordsInList == 0)
            {
                return -1.0;
            }
            // If we are here, then the list of password is not empty
            foreach (string CurrentPasswordInList in ListOfPasswords)
            {
                if (LevenshteinDistance(Password, CurrentPasswordInList) <= 1)
                {
                    Slist = 0.0; //Even if the password is distant just one from anyone in the list, it's a weak password
                    break;
                }
            }
            return Slist;
        }


        public static double LIST_strengthMetric(string Password, string ListOfPasswordsTxtFilePath)
        // This function tests the strength of the input Password against a list of provided banned passwords
        // (stored in a txt file whose name is provided as the secondo input variable), according to the LIST criterion.
        // Two double values can be returned: 10 (strong password) or 0 (weak password).
        // This function calls another one with the same name, but different signaure (overloading):
        //    static double LIST_strengthMetric(string Password, string[] ListOfPasswords)
        {
            string[] ListOfPasswordsAsStrings = ReturnListOfReadLinesInTxtFile_StringArray(ListOfPasswordsTxtFilePath);
            return LIST_strengthMetric_FromListAsArrayOfString(Password, ListOfPasswordsAsStrings);
        }

        ////////////////////////////////////////////
        // END OF ROUTINES ABOUT LIST_strengthMetric
        ////////////////////////////////////////////




        ////////////////////////////////////////////
        // BEGIN OF ROUTINES ABOUT BF_strengthMetric
        ////////////////////////////////////////////

        public static void FindTokens_LowerUpper(string InputString,
                                                 out int numberOfNum,
                                                 out int numberOfLow,
                                                 out int numberOfUpp,
                                                 out int numberOfSpe)
        // This function examines the InputString in order to check if 4 different types of characters are present.
        // There are 4 output int variables, each one for each type: numbers, lower chars, upper chars, special chars.
        // Regular expressions are exploited through the official standard library System.Text.RegularExpressions.
        // As it can be seen, Regex.Matches() return MatchCollection objects, that have Count as member variable to
        // understand how many occurrences are found.
        {
            string numExpr = @"[0-9]+";
            string lowerExpr = @"[a-z]+";
            string upperExpr = @"[A-Z]+";
            string specialExpr = "[^a-zA-Z_0-9]+";

            MatchCollection SetOfNum_Matches = Regex.Matches(InputString, numExpr);
            numberOfNum = SetOfNum_Matches.Count;

            MatchCollection SetOfLow_Matches = Regex.Matches(InputString, lowerExpr);
            numberOfLow = SetOfLow_Matches.Count;

            MatchCollection SetOfUpp_Matches = Regex.Matches(InputString, upperExpr);
            numberOfUpp = SetOfUpp_Matches.Count;

            MatchCollection SetOfSpe_Matches = Regex.Matches(InputString, specialExpr);
            numberOfSpe = SetOfSpe_Matches.Count;
        }


        public static double BF_strengthMetric(string Password,
                                        int LminNum,
                                        int LminLower,
                                        int LminUpper,
                                        int LminSpecial,
                                        int LminTwo,
                                        int LminThree,
                                        int LminAll)
        // This function tests the strength of the input Password according to the BF (i.e. BruteForce) criterion.
        // This function calls the function FindTokens_LowerUpper() to retrieve - in the input password - how many
        // occurrences are found, among 4 types of charactes: numbers, lower chars, upper chars, special chars.
        // Once those 4 numeric values are obtained, one of the additional 7 needed input parameters is used:
        // LminAll is the minimum password length if all 4 types of chars are present.
        // LminThree is the minimum password length if 3 out of 4 types of chars are present.
        // LminTwo is the minimum password length if 2 out of 4 types of chars are present.
        // Otherwise, if only one type of char is found in the password, than - according to the type - the minimum length
        // for the password is chosen among: LminNum, LminLower, LminUpper, LminSpecial.
        // Two double values can be returned: 10 (strong password) or 0 (weak password).
        // Of course, weak password is true when its length is less than the suitable input parameter Lmin*.
        {
            double Sbf = 10.0; //Sbf is the value to be returned, meaning the Strength according to BruteForce
            int PasswordLength = Password.Length;
            

            FindTokens_LowerUpper(Password,
                                  out int numberNum,
                                  out int numberLow,
                                  out int numberUpp,
                                  out int numberSpe);

            int TotalTypes = 0;
            if (numberNum > 0) TotalTypes++;
            if (numberLow > 0) TotalTypes++;
            if (numberUpp > 0) TotalTypes++;
            if (numberSpe > 0) TotalTypes++;

            switch (TotalTypes)
            {
                case 4:
                    if (PasswordLength < LminAll) Sbf = 0.0;
                    break;
                case 3:
                    if (PasswordLength < LminThree) Sbf = 0.0;
                    break;
                case 2:
                    if (PasswordLength < LminTwo) Sbf = 0.0;
                    break;
                case 1:
                    if (numberNum > 0)
                    {
                        if (PasswordLength < LminNum) Sbf = 0.0;
                    }
                    else if (numberLow > 0)
                    {
                        if (PasswordLength < LminLower) Sbf = 0.0;
                    }
                    else if (numberUpp > 0)
                    {
                        if (PasswordLength < LminUpper) Sbf = 0.0;
                    }
                    else // if (numberSpe > 0)
                    {
                        if (PasswordLength < LminSpecial) Sbf = 0.0;
                    }
                    break;
            }
            return Sbf;
        }

        //////////////////////////////////////////
        // END OF ROUTINES ABOUT BF_strengthMetric
        //////////////////////////////////////////

        ///////////////////////////////////////////////////////////////////
        // BEGIN OF ROUTINES ABOUT AdaptiveMemoryMarkovChain_strengthMetric
        ///////////////////////////////////////////////////////////////////

        // ------BEGIN V_Hashlist------

        //ASCII

        public static double MM_strengthMetric_Hashlist_Ascii(SMtrx1D M,
                                       string Password,
                                       int MemSize)
        {
            int NGRAM_max_size = (int)(Math.Pow(100, MemSize)); // Define the maximum size of a ngram for the input matrix
            // PLEASE NOTE: in C#, ^ is NOT the power operator! It's a logical XOR!!!

            int Transition_OFFSET = 28; // Offset to subtract to chars. Therefore, printable ASCII codes are translated
                                        // from the range [32..126] to the range [4..98].

            int Password_Start = 1;
            int Password_End = 99;
            double NoTransition = Math.Pow(10.0, -9.0); //Bonus if no transition AT ALL is found in the matrix
            double st = 1.0;    // Accumulated probability
            int ngram = Password_Start;
            int ngramLength;
            int ngram_Temp;
            int NextCharCode; // To store the code (with offset) of the next char in the password
            bool GotProbability; // Boolean to detect if we got a score for the transition
            double Probability;  // To store the value read in the transition matrix M

            int PasswordLength = Password.Length;

            for (int i = 0; i <= PasswordLength; i++) //We scan the input Password
            {

                if (i == PasswordLength) NextCharCode = Password_End;
                else NextCharCode = (int)Password[i] - Transition_OFFSET; // NOTE: index i because in Matlab is i+1 and Matlab has 1-based indices,
                                                                          //       while C# is 0-based.

                ngramLength = (int)Math.Ceiling(Math.Log10(ngram) / 2.0); //Compute the length of the ngram

                GotProbability = false;
                ngram_Temp = ngram;
                while (GotProbability == false)
                {
                    Probability = M.get(ngram_Temp - 1, NextCharCode - 1); // -1 is due because in C# indices are 0 based; in Matlab 1 based.
                    if (Probability > 0)
                    {
                        st = st * Probability / M.get(ngram_Temp - 1, 0); // Transition found, so we multiply it to the score.
                                                                      // NOTE: -1 because in Matlab indices are 1-based, while in C# are 0-based
                                                                      // NOTE: 0 because the first column of M is the total numbers of transitions
                                                                      //       seen during the training of a given n-gram
                        GotProbability = true; // We can exit the loop
                    }
                    else
                    {   // Here Probability is 0 --> No transition found.
                        ngramLength = ngramLength - 1; // We reduce the length
                        if (ngramLength <= 0)
                        {   // If the memory is 0
                            st = st * NoTransition; // We multiply by default bonus
                            GotProbability = true; // We can exit the loop
                        }
                        else
                        {
                            // We apply the adaptive bonus to the score of the password
                            if (M.get(ngram_Temp - 1, 2) > 0) // Index 2 because the 3rd column of M is the minimum value of the row associated to an n-gram
                            {
                                st = st * M.get(ngram_Temp - 1, 2); // We multiply by the lowest transition of the row of the current ngram
                            }
                            else
                            {
                                st = st * NoTransition; // Such ngram has never been seen so we put a potentially too huge bonus
                            }
                            ngram_Temp = ngram % (int)Math.Pow(100, ngramLength); // Catch the new ngram with the reduced size
                                                                                  // PLEASE NOTE: in C#, ^ is NOT the power operator! It's a logical XOR!!!

                            if (M.get(ngram_Temp - 1, 1) > 0) // Index 1 because the 2nd column of M is the maximum value of the row associated to an n-gram
                            {
                                st = st / M.get(ngram_Temp - 1, 1); // We divide by the highest transition of the row of the reduced ngram
                            }
                        }
                    }
                }

                ngram = (100 * ngram + NextCharCode) % NGRAM_max_size; // This function concatenates the new char on the right of the index.
            }
            st = -10.0 * Math.Log10(st);
            return st;
        }


        //Uni1K

        public static double MM_strengthMetric_Hashlist_Uni1K(SMtrx1D M,
                                               string Password,
                                               int MemSize)
        {
            // NOW EVERY CHARACTER IS ENCODED WITH A NUMBER THAT CAN BE UP TO
            // 1023 - 28 = 995(i.e. 996 combinations)

            int NGRAM_max_size = (int)(Math.Pow(1000, MemSize)); // Define the maximum size of a ngram for the input matrix
            // PLEASE NOTE: in C#, ^ is NOT the power operator! It's a logical XOR!!!
            
            int Transition_OFFSET = 28; // Offset to subtract to chars. Therefore, printable ASCII codes are translated
                                        // from the range [32..126] to the range [4..98].

            int Password_Start = 1;
            int Password_End = 999;
            double NoTransition = Math.Pow(10.0, -9.0); //Bonus if no transition AT ALL is found in the matrix
            double st = 1.0;    // Accumulated probability
            //UInt32 ngram = (UInt32) Password_Start;
            int ngram = Password_Start;
            int ngramLength;
            //UInt32 ngram_Temp;
            int ngram_Temp;
            int NextCharCode; // To store the code (with offset) of the next char in the password
            bool GotProbability; // Boolean to detect if we got a score for the transition
            double Probability;  // To store the value read in the transition matrix M

            int PasswordLength = Password.Length;

            for (int i = 0; i <= PasswordLength; i++) //We scan the input Password
            {

                if (i == PasswordLength) NextCharCode = Password_End;
                else NextCharCode = (int)Password[i] - Transition_OFFSET; // NOTE: index i because in Matlab is i+1 and Matlab has 1-based indices,
                                                                          //       while C# is 0-based.

                ngramLength = (int)Math.Ceiling(Math.Log10(ngram) / 3.0); //Compute the length of the ngram
                
                GotProbability = false;
                ngram_Temp = ngram;

                while (GotProbability == false)
                {
                    Probability = M.get(ngram_Temp - 1, NextCharCode - 1); // -1 is due because in C# indices are 0 based; in Matlab 1 based.
                    if (Probability > 0)
                    {
                        st = st * Probability / M.get(ngram_Temp - 1, 0); // Transition found, so we multiply it to the score.
                                                                      // NOTE: -1 because in Matlab indices are 1-based, while in C# are 0-based
                                                                      // NOTE: 0 because the first column of M is the total numbers of transitions
                                                                      //       seen during the training of a given n-gram
                        GotProbability = true; // We can exit the loop
                    }
                    else
                    {   // Here Probability is 0 --> No transition found.
                        ngramLength = ngramLength - 1; // We reduce the length
                        if (ngramLength <= 0)
                        {   // If the memory is 0
                            st = st * NoTransition; // We multiply by default bonus
                            GotProbability = true; // We can exit the loop
                        }
                        else
                        {
                            // We apply the adaptive bonus to the score of the password
                            if (M.get(ngram_Temp - 1, 2) > 0) // Index 2 because the 3rd column of M is the minimum value of the row associated to an n-gram
                            {
                                st = st * M.get(ngram_Temp - 1, 2); // We multiply by the lowest transition of the row of the current ngram
                            }
                            else
                            {
                                st = st * NoTransition; // Such ngram has never been seen so we put a potentially too huge bonus
                            }
                            ngram_Temp = ngram % (int)Math.Pow(1000, ngramLength); // Catch the new ngram with the reduced size
                                                                                   // PLEASE NOTE: in C#, ^ is NOT the power operator! It's a logical XOR!!!

                            if (M.get(ngram_Temp - 1, 1) > 0) // Index 1 because the 2nd column of M is the maximum value of the row associated to an n-gram
                            {
                                st = st / M.get(ngram_Temp - 1, 1); // We divide by the highest transition of the row of the reduced ngram
                            }
                        }
                    }
                }

                ngram = (int)((1000.0 * (double)ngram + NextCharCode) % NGRAM_max_size); // This function concatenates the new char on the right of the index.
                // NOTE: the cast (double) above is necessary, otherwise when ngram becomes larger it overflows during this update!!!
                //       E.g.:  (1000 * 5005086 + 21) % 1000000000 

            }
            st = -10.0 * Math.Log10(st);
            return st;
        }
        // ------END HASHLIST------

        public static double AdaptiveHMM_normalize(double Score)
        // We do not need to test if score==inf because the formula is built so
        // that when score is a big number, then scoreNorm becomes 5+5 = 10.
        {
            // double meanNormalize = 162.6786;
            // double stdevNormalize=1; // OLD value: 56.1706
            double scoreNorm = 5.0 * Math.Tanh(0.01 * (Score - 162.6786) / 1) + 5.0;
            return scoreNorm;
        }


        /////////////////////////////////////////////////////////////////
        // END OF ROUTINES ABOUT AdaptiveMemoryMarkovChain_strengthMetric
        /////////////////////////////////////////////////////////////////

        ///////////////////////////////////////////////
        // BEGIN OF ROUTINES ABOUT CFHMM_strengthMetric
        ///////////////////////////////////////////////

        public static void FindTokens(string InputString,
                                      out int lengthPasswordTokens,
                                      out int[] vectLengthTokensNum,
                                      out int[] vectLengthTokensAlfa,
                                      out int[] vectLengthTokensSpecial,
                                      out int[] vectInds_Final)
        // This function examines the InputString in order to check if 3 different types of characters are present:
        // numbers, alphanumeric chars, special chars.
        // Regular expressions are exploited through the official standard library System.Text.RegularExpressions.
        // As it can be seen, Regex.Matches() return MatchCollection objects, that have Count as member variable to
        // understand how many occurrences are found.
        {
            string numExpr = @"[0-9]+";
            string alfaExpr = @"[a-zA-Z]+";
            string specialExpr = "[^a-zA-Z_0-9]+";

            int maxLengthNumTokens = 12;
            int maxLengthAlfaTokens = 20;

            int numberOfNum, numberOfAlfa, numberOfSpe;

            int[] vectIndsTokensNum, vectIndsTokensAlfa, vectIndsTokensSpe;
            int[] vectStartIndicesNum;     // In Matlab: numStartInd
            int[] vectStartIndicesAlfa;    // In Matlab: alfaStartInd
            int[] vectStartIndicesSpecial; // In Matlab: specialStartInd

            int i; // Index for vectors

            //In Matlab code, the test password was Password  = @"!!r1s4S100juega$$";

            MatchCollection SetOfNum_Matches = Regex.Matches(InputString, numExpr);
            numberOfNum = SetOfNum_Matches.Count;
            vectLengthTokensNum = new int[numberOfNum];
            vectStartIndicesNum = new int[numberOfNum];
            i = 0;
            foreach (Match CurrentMatch in SetOfNum_Matches)
            {

                vectLengthTokensNum[i] = CurrentMatch.Value.Length;
                vectStartIndicesNum[i] = CurrentMatch.Index; // NOTE: here index is 0 based, while in Matlab is 1-based

                i++;
            }



            MatchCollection SetOfAlfa_Matches = Regex.Matches(InputString, alfaExpr);
            numberOfAlfa = SetOfAlfa_Matches.Count;
            vectLengthTokensAlfa = new int[numberOfAlfa];
            vectStartIndicesAlfa = new int[numberOfAlfa];
            i = 0;
            foreach (Match CurrentMatch in SetOfAlfa_Matches)
            {
                vectLengthTokensAlfa[i] = CurrentMatch.Value.Length;
                vectStartIndicesAlfa[i] = CurrentMatch.Index; // NOTE: here index is 0 based, while in Matlab is 1-based
                i++;
            }



            MatchCollection SetOfSpe_Matches = Regex.Matches(InputString, specialExpr);
            numberOfSpe = SetOfSpe_Matches.Count;
            vectLengthTokensSpecial = new int[numberOfSpe];
            vectStartIndicesSpecial = new int[numberOfSpe];
            i = 0;
            foreach (Match CurrentMatch in SetOfSpe_Matches)
            {
                vectLengthTokensSpecial[i] = CurrentMatch.Value.Length;
                vectStartIndicesSpecial[i] = CurrentMatch.Index; // NOTE: here index is 0 based, while in Matlab is 1-based

                i++;
            }

            lengthPasswordTokens = numberOfNum + numberOfAlfa + numberOfSpe; // Password length measured in tokens

            // Let's copy vectLengthTokensNum in vectIndsTokensNum
            vectIndsTokensNum = new int[vectLengthTokensNum.Length];
            Array.Copy(vectLengthTokensNum, vectIndsTokensNum, vectLengthTokensNum.Length);

            // Let's create vectIndsTokensAlfa from vectLengthTokensAlfa
            vectIndsTokensAlfa = new int[vectLengthTokensAlfa.Length];
            for (i = 0; i < vectLengthTokensAlfa.Length; i++)
            {
                vectIndsTokensAlfa[i] = vectLengthTokensAlfa[i] + maxLengthNumTokens;
            }

            // Let's create vectIndsTokensSpe from vectLengthTokensSpe
            vectIndsTokensSpe = new int[vectLengthTokensSpecial.Length];
            for (i = 0; i < vectLengthTokensSpecial.Length; i++)
            {
                vectIndsTokensSpe[i] = vectLengthTokensSpecial[i] + maxLengthNumTokens + maxLengthAlfaTokens;
            }

            // Position of the tokens
            // 1) Let's merge the indices: vectAllStartInd = [vectIndsTokensAlfa vectIndsTokensNum vectIndsTokensSpe ] in Matlab
            int[] vectAllStartInd = new int[lengthPasswordTokens];
            vectStartIndicesAlfa.CopyTo(vectAllStartInd, 0); // NOTE: the alternative quickest way is to use for loop like in C language
            vectStartIndicesNum.CopyTo(vectAllStartInd, vectStartIndicesAlfa.Length);
            vectStartIndicesSpecial.CopyTo(vectAllStartInd, vectStartIndicesAlfa.Length + vectStartIndicesNum.Length);
            // 2) Let's sort (in place) this vector
            Array.Sort(vectAllStartInd);
            // 3) Let's find the intersection
            // MATLAB instruction to translate: [~,orderTokensNum,~] = intersect(allStartInd,numStartInd);
            // or better, with our C# variables:[~,vectOrderTokensNum,~] = intersect(vectAllStartInd,vectStartIndicesNum);
            //var IntersectIndicesWithNum = vectAllStartInd.Intersect(vectStartIndicesNum); // Using Linq to perform intersect;
            // IEnumerable<int> IntersectIndicesWithNum_IE = vectAllStartInd.Intersect(vectStartIndicesNum); //Using Linq with IEnumerable
            HashSet<int> remainingRemovingStartIndicesNum = new HashSet<int>(vectStartIndicesNum);
            List<int> listOrderTokensNum = new List<int>();
            for (i = 0; i < vectAllStartInd.Length; i++)
            {
                if (remainingRemovingStartIndicesNum.Remove(vectAllStartInd[i]))
                {
                    listOrderTokensNum.Add(i);
                }
            }
            int[] vectOrderTokensNum = listOrderTokensNum.ToArray();

            // MATLAB instruction to translate: [~,orderTokensAlfa,~] = intersect(allStartInd,alfaStartInd);
            // or better, with our C# variables:[~,vectOrderTokensAlfa,~] = intersect(vectAllStartInd,vectStartIndicesAlfa);
            HashSet<int> remainingRemovingStartIndicesAlfa = new HashSet<int>(vectStartIndicesAlfa);
            List<int> listOrderTokensAlfa = new List<int>();
            for (i = 0; i < vectAllStartInd.Length; i++)
            {
                if (remainingRemovingStartIndicesAlfa.Remove(vectAllStartInd[i]))
                {
                    listOrderTokensAlfa.Add(i);
                }
            }
            int[] vectOrderTokensAlfa = listOrderTokensAlfa.ToArray();

            // MATLAB instruction to translate: [~,orderTokensSpecial,~] = intersect(allStartInd,specialStartInd);
            // or better, with our C# variables:[~,vectOrderTokensSpecial,~] = intersect(vectAllStartInd,vectStartIndicesSpecial);
            HashSet<int> remainingRemovingStartIndicesSpecial = new HashSet<int>(vectStartIndicesSpecial);
            List<int> listOrderTokensSpecial = new List<int>();
            for (i = 0; i < vectAllStartInd.Length; i++)
            {
                if (remainingRemovingStartIndicesSpecial.Remove(vectAllStartInd[i]))
                {
                    listOrderTokensSpecial.Add(i);
                }
            }
            int[] vectOrderTokensSpecial = listOrderTokensSpecial.ToArray();
            // END: Position of the tokens

            // Vector with the tokens order and vector with their indices
            int[] vectOrder = new int[lengthPasswordTokens];
            vectOrderTokensNum.CopyTo(vectOrder, 0); // NOTE: the alternative quickest way is to use for loop like in C language
            vectOrderTokensAlfa.CopyTo(vectOrder, vectOrderTokensNum.Length);
            vectOrderTokensSpecial.CopyTo(vectOrder, vectOrderTokensNum.Length + vectOrderTokensAlfa.Length);

            // Let's translate the Matlab instruction: vectInds=[indsTokensNum indsTokensAlfa indsTokensSpecial];
            int[] vectIndices = new int[lengthPasswordTokens];
            vectIndsTokensNum.CopyTo(vectIndices, 0);
            vectIndsTokensAlfa.CopyTo(vectIndices, vectIndsTokensNum.Length);
            vectIndsTokensSpe.CopyTo(vectIndices, vectIndsTokensNum.Length + vectIndsTokensAlfa.Length);

            // Let's translate the Matlab instruction: [~,sortedInds]=sort(vectOrder);
            int[] sortedIndices = new int[lengthPasswordTokens];
            for (i = 0; i < lengthPasswordTokens; i++) sortedIndices[i] = i; // Create a vector of integer 0:lengthPasswordTokens-1
            Array.Sort(vectOrder, sortedIndices); // Here vectOrder is ordered and sortedIndices is filled properly

            // Let's translate the Matlab instruction: vectInds=vectInds(sortedInds);
            // in order to use the column indices from sort(vectOrder) to sort vectInds
            vectInds_Final = new int[lengthPasswordTokens];
            for (i = 0; i < lengthPasswordTokens; i++) vectInds_Final[i] = vectIndices[sortedIndices[i]];

        }

        public static bool FindIfAtLeastOneElementInTheInputVectorIsLargerThanThreshold_int(int[] InputVector, int Threshold)
        {
            bool Result = false;
            for (int i=0; i<InputVector.Length; i++)
            {
                if (InputVector[i]> Threshold)
                {
                    Result = true;
                    break;
                }
            }

            return Result;
        }

        public static double ReturnTheMultiplicationOfAllTheElementsInAVector_Double(double[] InputVector)
        {
            double Result = 1.0;
            for (int i=0; i < InputVector.Length; i++)
            {
                Result = Result * InputVector[i];
            }
            return Result;
        }

        public static int[] CreateVectLengthTokensForCFHMM(int MaxLengthOfCharactersInEveryNumToken, 
            int MaxLengthOfCharactersInEveryAlphaToken, int MaxLengthOfCharactersInEverySpecialToken)

        {
            // This function translates the Matlab instruction: vectLengthTokens=[1:maxLengthNumTokens 1:maxLengthAlfaTokens 1:maxLengthSpecialTokens];
            // NOTE: this is the best version in terms of efficiency (speed, buffer allocation,...) but we could also use:
            //          int[] Part1 = Enumerable.Range(1, MaxLengthOfCharactersInEveryNumToken).ToArray();
            //          int[] Part2 = Enumerable.Range(1, MaxLengthOfCharactersInEveryAlphaToken).ToArray();
            //          int[] Part3 = Enumerable.Range(1, MaxLengthOfCharactersInEverySpecialToken).ToArray();
            //          OutputVector = (Part1.Concat(Part2).ToArray()).Concat(Part3).ToArray();

            int[] OutputVector = new int[MaxLengthOfCharactersInEveryNumToken 
                + MaxLengthOfCharactersInEveryAlphaToken + MaxLengthOfCharactersInEverySpecialToken];
            int i;
            // Let's create 1:MaxLengthOfCharactersInEveryNumToken in the first part of the output vector
            for (i=0; i< MaxLengthOfCharactersInEveryNumToken; i++)
            {
                OutputVector[i] = i + 1;
            }
            // Let's create 1:MaxLengthOfCharactersInEveryAlphaToken in the second part of the output vector
            for (i = 0; i < MaxLengthOfCharactersInEveryAlphaToken; i++)
            {
                OutputVector[MaxLengthOfCharactersInEveryNumToken + i] = i + 1;
            }
            // Let's create 1:MaxLengthOfCharactersInEverySpecialToken in the third part of the output vector
            for (i = 0; i < MaxLengthOfCharactersInEverySpecialToken; i++)
            {
                OutputVector[MaxLengthOfCharactersInEveryNumToken + MaxLengthOfCharactersInEveryAlphaToken + i] = i + 1;
            }

            return OutputVector;
        }

        public static double CFHMM_strengthMetric_V2(string Password,string FileNameOfCFHMM_TrainingData)
        {
            double Sn = 0.0;

            int i, j; // To browse the matrices


            // Let's translate the Matlab instruction: load CFHMM_probs
            // Used Libraries:
            //  - To load matrices (MatlabReader.*):  MathNet.Numerics.Data.Matlab (Matlab IO extensions for Math.Net) --> Matrix<double>
            //  - To load tensors  (MatFileReader.*): DotNetDoctor.csmatio --> MLDouble


            MLDouble maxLengthNumTokens_MatScalar = LoadScalarFromMatlabFile_Double(FileNameOfCFHMM_TrainingData, "maxLengthNumTokens");
            int maxLengthNumTokens_int = (int)maxLengthNumTokens_MatScalar.Get(0); // 12


            MLDouble maxLengthAlfaTokens_MatScalar = LoadScalarFromMatlabFile_Double(FileNameOfCFHMM_TrainingData, "maxLengthAlfaTokens");
            int maxLengthAlfaTokens_int = (int)maxLengthAlfaTokens_MatScalar.Get(0); // 20

            MLDouble maxLengthSpecialTokens_MatScalar = LoadScalarFromMatlabFile_Double(FileNameOfCFHMM_TrainingData, "maxLengthSpecialTokens");
            int maxLengthSpecialTokens_int = (int)maxLengthSpecialTokens_MatScalar.Get(0); //6

            MLDouble TypeOfSupportedCharset_MatScalar = LoadScalarFromMatlabFile_Double(FileNameOfCFHMM_TrainingData, "TypeOfSupportedCharset");
            int TypeOfSupportedCharset_int = (int)TypeOfSupportedCharset_MatScalar.Get(0); ;

            int[] vectLengthTokens = CreateVectLengthTokensForCFHMM(maxLengthNumTokens_int, maxLengthAlfaTokens_int, maxLengthSpecialTokens_int);
            // The above instruction is the generalization of: int[] vectLengthTokens = CreateVectLengthTokensForCFHMM(12, 20, 6), that produces:
            // int[] vectLengthTokens = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12,
            //                           1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
            //                           1, 2, 3, 4, 5, 6};

            Matrix<double> matProb_First_Alfa = MatlabReader.Read<double>(FileNameOfCFHMM_TrainingData, "matProb_First_Alfa");
            Matrix<double> matProb_First_Grammar = MatlabReader.Read<double>(FileNameOfCFHMM_TrainingData, "matProb_First_Grammar");
            Matrix<double> matProb_First_Nums = MatlabReader.Read<double>(FileNameOfCFHMM_TrainingData, "matProb_First_Nums");
            Matrix<double> matProb_First_Special = MatlabReader.Read<double>(FileNameOfCFHMM_TrainingData, "matProb_First_Special");
            Matrix<double> vectProb_Length_Grammar = MatlabReader.Read<double>(FileNameOfCFHMM_TrainingData, "vectProb_Length_Grammar");
            MLDouble matProb_Trans_Tokens = LoadTensorFromMatlabFile_Double(FileNameOfCFHMM_TrainingData,"matProb_Trans_Tokens");
            // NOTE: matProb_Trans_Grammar will be read later in the code

            MLDouble CodePointOffset_MatScalar = LoadScalarFromMatlabFile_Double(FileNameOfCFHMM_TrainingData, "CodePointOffset");
            int CodePointOffset_int = (int)CodePointOffset_MatScalar.Get(0); // 12



            // Let's translate: matProb_First=[matProb_First_Nums ; matProb_First_Alfa ; matProb_First_Special];
            Matrix<double> matProb_First = new MathNet.Numerics.LinearAlgebra.Double.DenseMatrix(matProb_First_Nums.RowCount +
                                                                                matProb_First_Alfa.RowCount +
                                                                                matProb_First_Special.RowCount, matProb_First_Nums.ColumnCount);
            int NumberOfCommonColumns = matProb_First.ColumnCount;
            for (i = 0; i < matProb_First_Nums.RowCount; i++)
            {
                for (j = 0; j < NumberOfCommonColumns; j++)
                {
                    matProb_First[i, j] = matProb_First_Nums[i, j];
                }
            }
            for (i = 0; i < matProb_First_Alfa.RowCount; i++)
            {
                for (j = 0; j < NumberOfCommonColumns; j++)
                {
                    matProb_First[i + matProb_First_Nums.RowCount, j] = matProb_First_Alfa[i, j];
                }
            }
            for (i = 0; i < matProb_First_Special.RowCount; i++)
            {
                for (j = 0; j < NumberOfCommonColumns; j++)
                {
                    matProb_First[i + matProb_First_Nums.RowCount + matProb_First_Alfa.RowCount, j] = matProb_First_Special[i, j];
                }
            }

            // Let's translate: structTokens = findTokens(password);
            // I decided to not use the variable structTokens in C#, but to expect findTokens() to return the 5 fields of structTokens
            // that are really used in the caller function, i.e. CFHMM_strengthMetrics:
            // (1) lengthPwrTokens     --> lengthPasswordTokens
            // (2) lengthTokensNum     --> vectLengthTokensNum
            // (3) lengthTokensAlfa    --> vectLengthTokensAlfa
            // (4) lengthTokensSpecial --> vectLengthTokensSpecial
            // (5) vectInds            --> vectInds_Final    

            int lengthOfPassword = Password.Length;

            FindTokens(Password,
                       out int lengthPasswordTokens,
                       out int[] vectLengthTokensNum,
                       out int[] vectLengthTokensAlfa,
                       out int[] vectLengthTokensSpecial,
                       out int[] vectInds_Final);
            // Let's read the suitable tensor that in Matlab code is one of the elements of the cell array cellProb_Trans_Grammar
            MLDouble tensorMatProbs = LoadTensorFromMatlabFile_Double(FileNameOfCFHMM_TrainingData,
                                                    "matProb_Trans_Grammar" + lengthPasswordTokens.ToString());

              
            bool Flag_numsTooLong = FindIfAtLeastOneElementInTheInputVectorIsLargerThanThreshold_int(vectLengthTokensNum, maxLengthNumTokens_int);              // ,12
            bool Flag_alfasTooLong = FindIfAtLeastOneElementInTheInputVectorIsLargerThanThreshold_int(vectLengthTokensAlfa, maxLengthAlfaTokens_int);           // ,20
            bool Flag_specialTooLong = FindIfAtLeastOneElementInTheInputVectorIsLargerThanThreshold_int(vectLengthTokensSpecial, maxLengthSpecialTokens_int);   // ,6
            bool Flag_passwordTooLong = (lengthPasswordTokens > 12) ? true : false; // lengthPasswordTokens is a scalar, not a vector like the previous three ones

            if (Flag_numsTooLong || Flag_alfasTooLong || Flag_specialTooLong || Flag_passwordTooLong)
            {
                Sn = 0.0;
                return Sn;
            }
            else
            {
                // WE COMPUTE THE PROBABILITIES FOR THE GENERAL GRAMMAR
                double[] vectProbsGrammar = new double[lengthPasswordTokens]; // Note: in C# this instruction set all the elements to zeros


                // Find the minimum non-zero probability in the initialization vector

                double minNonZeroProbFirst = ReturnTheMinimumNonZeroValueInARowOfAMatrix_Double(matProb_First_Grammar, lengthPasswordTokens - 1); // - 1 because in C# indices are 0-based

                vectProbsGrammar[0] = matProb_First_Grammar[lengthPasswordTokens - 1, vectInds_Final[0] - 1]; // - 1 because in C# indices are 0-based

                if (vectProbsGrammar[0] == 0.0)
                {
                    vectProbsGrammar[0] = minNonZeroProbFirst / 10.0;
                }

                // Find the minimum non-zero probability in the matrix
                double minNonZeroProb = ReturnTheMinimumNonZeroValueInATensor_Double(tensorMatProbs);

                for (i = 0; i < lengthPasswordTokens - 1; i++)
                {
                    int currentTokenRow = vectInds_Final[i] - 1; // In C# indices are 0-based, therefore -1 is needed
                    int nextTokenCol = vectInds_Final[i + 1] - 1;  // In C# indices are 0-based, therefore -1 is needed
                    double probability = ReturnTensorValue_Double(tensorMatProbs, currentTokenRow, nextTokenCol, i);

                    // If the corresponding probability is zero, we assign it a value lower than the lowest non-zero probability in the matrix
                    if (probability == 0)
                    {
                        vectProbsGrammar[i + 1] = minNonZeroProb / 10.0;
                    }
                    else
                    {
                        vectProbsGrammar[i + 1] = probability;
                    }
                }

                // WE COMPUTE THE PROBABILITIES FOR EACH TOKEN
                int[] passwordNum = ConvertStringInUnicode1KIntCodePoints(Password);
                // NOTE: The following is not suitable because UTF8 uses up to 4 bytes for each character, while we want an integer for each character.
                //      byte[] passwordNum = Encoding.UTF8.GetBytes(Password); // To translate the Matlab code: passwordNum=double(password);
                // NOTE: The following is not suitable because non-ASCII characters are mapped into ASCII characters (values from 0 to 127):
                //      Byte[] passwordNum = Encoding.ASCII.GetBytes(Password); 


                double[] vectProbsCharacters = new double[lengthOfPassword]; // Translates the Matlab code: vectProbsCharacters=zeros(1,lengthPwr);
                int contCharactersTotal = 0;
                for (int contTokens = 1; contTokens <= lengthPasswordTokens; contTokens++)
                {
                    contCharactersTotal++;

                    int lengthToken = vectLengthTokens[vectInds_Final[contTokens - 1] - 1]; // -1 because in C# the indices are 0-based
                    vectProbsCharacters[contCharactersTotal - 1] =                          // -1 because in C# the indices are 0-based
                            matProb_First[vectInds_Final[contTokens - 1] - 1, passwordNum[contCharactersTotal - 1] - CodePointOffset_int - 1]; // -1 because in C# the indices are 0-based

                    minNonZeroProb = ReturnTheMinimumNonZeroValueInASliceOfATensor_Double(matProb_Trans_Tokens, lengthToken - 1);

                    // FOR contCharactersToken=1:lengthToken-1
                    for (int contCharactersToken = 1; contCharactersToken <= lengthToken - 1; contCharactersToken++)
                    {
                        contCharactersTotal++;

                        int currentCharacterRow = passwordNum[contCharactersTotal - 1 - 1] - CodePointOffset_int - 1;
                        int nextCharacterCol = passwordNum[contCharactersTotal - 1] - CodePointOffset_int - 1;
                        double probability = ReturnTensorValue_Double(matProb_Trans_Tokens, currentCharacterRow, nextCharacterCol, lengthToken - 1);
                        // If the corresponding probability is zero, we assign it a value lower than the lowest non-zero probability in the matrix
                        if (probability == 0)
                        {
                            vectProbsCharacters[contCharactersTotal - 1] = minNonZeroProb / 10;
                        }
                        else
                        {
                            vectProbsCharacters[contCharactersTotal - 1] = probability;
                        }
                    }
                }

                double probPassword = ReturnTheMultiplicationOfAllTheElementsInAVector_Double(vectProbsGrammar) *
                                       ReturnTheMultiplicationOfAllTheElementsInAVector_Double(vectProbsCharacters);

                Sn = -Math.Log10(probPassword);

                return Sn;
            }
        }

        public static double CFHMM_normalize(double Score)
        // We do not need to test if score==inf because the formula is built so
        // that when score is a big number, then scoreNorm becomes 5+5 = 10.
        {
            // double meanNormalize = 13.4383;
            // double stdevNormalize=0.15; // OLD value: 3.91
            double Sn_norm = 5.0 * Math.Tanh(0.01 * (Score - 13.4383) / 0.15) + 5.0;
            return Sn_norm;
        }
        

        /////////////////////////////////////////////
        // END OF ROUTINES ABOUT CFHMM_strengthMetric
        /////////////////////////////////////////////


        ///////////////////////////////////////////////////
        // BEGIN OF ROUTINES ABOUT SimpleHMM_strengthMetric
        ///////////////////////////////////////////////////

        public static double SimpleHMM_strengthMetric_V2(string Password,
                                          string FileNameOfSHMM_TrainingData)
        {
            double Ss;

            int i; // Counter for loops

            // Let's translate the Matlab instruction: load simpleHMM_probs
            // Used Libraries:
            //  - To load scalars  (MatFileReader.*): DotNetDoctor.csmatio-- > MLDouble
            //  - To load vectors  (MatlabReader.*):  MathNet.Numerics.Data.Matlab (Matlab IO extensions for Math.Net) --> Matrix<double>
            //  - To load matrices (MatlabReader.*):  MathNet.Numerics.Data.Matlab (Matlab IO extensions for Math.Net) --> Matrix<double>
            //  - To load tensors  (MatFileReader.*): DotNetDoctor.csmatio --> MLDouble



            MLDouble matProb_Trans = LoadTensorFromMatlabFile_Double(FileNameOfSHMM_TrainingData, "matProb_Trans");

            Matrix<double> matProb_First = MatlabReader.Read<double>(FileNameOfSHMM_TrainingData, "matProb_First");

            //Matrix<double> vectProb_Length = MatlabReader.Read<double>(FileNameOfSHMM_TrainingData, "vectProb_Length");

            MLDouble lengthMin_MatScalar = LoadScalarFromMatlabFile_Double(FileNameOfSHMM_TrainingData, "lengthMin");
            int lengthMin = (int)lengthMin_MatScalar.Get(0);

            MLDouble lengthMax_MatScalar = LoadScalarFromMatlabFile_Double(FileNameOfSHMM_TrainingData, "lengthMax");
            int lengthMax = (int)lengthMax_MatScalar.Get(0);

            MLDouble CodePointOffset_MatScalar = LoadScalarFromMatlabFile_Double(FileNameOfSHMM_TrainingData, "CodePointOffset");
            int CodePointOffset = (int)CodePointOffset_MatScalar.Get(0);


            MLDouble TypeOfSupportedCharset_MatScalar = LoadScalarFromMatlabFile_Double(FileNameOfSHMM_TrainingData, "TypeOfSupportedCharset");
            int TypeOfSupportedCharset_int = (int)TypeOfSupportedCharset_MatScalar.Get(0);



            int[] passwordNum = ConvertStringInUnicode1KIntCodePoints(Password);
            // NOTE: The following is not suitable because UTF8 uses up to 4 bytes for each character, while we want an integer for each character.
            //      byte[] passwordNum = Encoding.UTF8.GetBytes(Password); // To translate the Matlab code: passwordNum=double(password);
            // NOTE: The following is not suitable because non-ASCII characters are mapped into ASCII characters (values from 0 to 127):
            //      Byte[] passwordNum = Encoding.ASCII.GetBytes(Password); 

            int LengthOfPasswordInChars = Password.Length;
            int selectProbs = LengthOfPasswordInChars;
            if (selectProbs > lengthMax) selectProbs = lengthMax;
            else if (selectProbs < lengthMin) selectProbs = lengthMin;

            double[] vectProbs = new double[LengthOfPasswordInChars]; // Translates the Matlab code: vectProbs=zeros(1,lengthPwr);

            // find the minimum non-zero probability in the initialization vector
            double minNonZeroProbFirst = ReturnTheMinimumNonZeroValueInARowOfAMatrix_Double(matProb_First, selectProbs - 1); // NOTE: - 1 because Matlab has 1-based indices, while C# is 0-based.
            // The above instruction performed the following instructions:
            //      vectProbsFirst = matProb_First(selectProbs,:);
            //      minNonZeroProbFirst=min(vectProbsFirst(vectProbsFirst>0));

            // Let's translate Matlab: vectProbs(1) = vectProbsFirst(passwordNum(1) - CodePointOffset);
            vectProbs[0] = matProb_First[selectProbs - 1, passwordNum[0] - CodePointOffset - 1]; // NOTE: Every '-1' because Matlab has 1-based indices, while C# is 0-based.
            if (vectProbs[0] == 0) vectProbs[0] = minNonZeroProbFirst / 10.0;

            // LET'S FIND THE MINIMUM NONZERO VALUE OF matProbs
            double minimoNonZeroProb = ReturnTheMinimumNonZeroValueInASliceOfATensor_Double(matProb_Trans, selectProbs - 1);

            for (i = 2; i <= LengthOfPasswordInChars; i++) // We begin with i=2 because we want to adhere to Matlab code instructions
            {
                int currentCharacterRow = passwordNum[i - 2] - CodePointOffset;
                int nextCharacterCol = passwordNum[i - 1] - CodePointOffset;
                double Probability = ReturnTensorValue_Double(matProb_Trans, currentCharacterRow - 1, nextCharacterCol - 1, selectProbs - 1);
                if (Probability == 0) Probability = minimoNonZeroProb / 10;
                vectProbs[i - 1] = Probability;
            }

            double probPassword = ReturnTheMultiplicationOfAllTheElementsInAVector_Double(vectProbs);

            Ss = -Math.Log10(probPassword);


            return Ss;
        }



        public static double SimpleHMM_normalize(double Score)
        // We do not need to test if score==inf because the formula is built so
        // that when score is a big number, then scoreNorm becomes 5+5 = 10.
        {
            // double meanNormalize = 13.0325;
            // double stdevNormalize=0.15; // OLD value: 3.91
            double Sn_norm = 5.0 * Math.Tanh(0.01 * (Score - 13.0325) / 0.15) + 5.0;
            return Sn_norm;
        }


        /////////////////////////////////////////////////
        // END OF ROUTINES ABOUT SimpleHMM_strengthMetric
        /////////////////////////////////////////////////


        public static double FusionWS(double Slist, double Sbf, double Sa, double Sn)
        {
            double Sfinal=-100.0;
            if (Slist == 0.0 || Sbf == 0.0)
            {
                Sfinal = 0.05 * Sa + 0.05 * Sn;
            }

            if (Slist != 0.0 && Sbf != 0.0)
            {
                Sfinal = 0.7 * Sa + 0.3 * Sn;
            }
            return Sfinal;
        }

    } // End class PasswordStrength
} // end namespace Library_PasswordStrength 