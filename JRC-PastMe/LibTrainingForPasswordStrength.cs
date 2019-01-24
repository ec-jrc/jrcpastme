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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Data.Matlab; // Needed for class MatlabWriter
//using MathNet.Numerics.LinearAlgebra.Double;
using DotNetDoctor.csmatio; // Needed for class MatFileWriter
using DotNetDoctor.csmatio.io;
using DotNetDoctor.csmatio.types;
using System.IO;
using System.Text.RegularExpressions; //Useful for regual expressions FindTokensForTraining
using System.Diagnostics; //Useful for StopWatch and TimeSpan classes (in JRC_TextProgressInfoV4_* functions)



namespace JRC_PastMe
{
    public class TrainingForPasswordStrength
    {

        public static bool IsASCII(string StringToTest)
        {
            // ASCII encoding replaces non-ascii with question marks, so we use UTF8 to see if multi-byte sequences are there
            return Encoding.UTF8.GetByteCount(StringToTest) == StringToTest.Length;
        }


        public static bool CharIsPrintableASCII(char InputChar)
        {
            bool CharIsPrintableASCII = true;
            if (((int)InputChar > 126) || ((int)InputChar < 32))
            {
                CharIsPrintableASCII = false;
            }
            return CharIsPrintableASCII;
        }


        public static bool CharIsNotPrintableASCII(char InputChar)
        {
            bool CharIsNotPrintableASCII = false;
            if (((int)InputChar > 126) || ((int)InputChar < 32))
            {
                CharIsNotPrintableASCII = true;
            }
            return CharIsNotPrintableASCII;
        }


        public static bool PasswordIsCorrectAndNotEmpty(string PasswordToTest)
        {
            bool PasswordIsCorrect = true;
            if (PasswordToTest.Length == 0) PasswordIsCorrect = false;
            else
            {
                foreach (char CurrentChar in PasswordToTest)
                {
                    if (CharIsNotPrintableASCII(CurrentChar))
                    {
                        PasswordIsCorrect = false;
                        break;
                    }
                }

            }

            //StringBuilder BuiltStringToTest = new StringBuilder(PasswordToTest.Length);
            return PasswordIsCorrect;
        }


        public static int[] FindRowsOfMatrixHavingNonZeroValues(Matrix<double> InputMatrix, bool ShowTextProgressInfo)
        {   // Generate the vector containing the number of the rows that have at least one non-zero element.
            // Please note: this is different from the find (Matlab command), since here we do not repeat the row index if there are more than one
            //              non-zero element.
            int NumberOfRows = InputMatrix.RowCount;
            int NumberOfCols = InputMatrix.ColumnCount;
            bool[] VectorOfFlags = new bool[NumberOfRows]; //FlagRowsHavingNonZeroElements has length equal to the number of rows (InputMatrix.RowCount)
            int i, j;
            int NumberOfTrues = 0;


            if (ShowTextProgressInfo)
            {
                Console.WriteLine("FindRowsOfMatrixHavingNonZeroValues - Phase 1 of 2");
            }

            for (i = 0; i < NumberOfRows; i++)
            {
                for (j = 0; j < NumberOfCols; j++)
                {
                    if (InputMatrix[i, j] != 0)
                    {
                        VectorOfFlags[i] = true;
                        NumberOfTrues++;
                        break; //Go to the next row (new value of i, neglecting remaining columns of the current row)
                    }
                }
            }

            if (ShowTextProgressInfo)
            {
                Console.WriteLine("FindRowsOfMatrixHavingNonZeroValues - Phase 2 of 2");
            }

            int[] VectorOfRowsHavingNonZeroValues = new int[NumberOfTrues];

            j = 0;
            for (i = 0; i < NumberOfRows; i++)
            {
                if (VectorOfFlags[i])
                {
                    VectorOfRowsHavingNonZeroValues[j] = i;
                    j++;
                }
            }

            if (ShowTextProgressInfo)
            {
                Console.WriteLine("FindRowsOfMatrixHavingNonZeroValues - Completed");
            }
            return VectorOfRowsHavingNonZeroValues;
        }

        public static void ReturnMinAndMaxOfNonZeroElementsInHashlist(SMtrx1D InputHashlist, int RowToScan, int NbColumn, int ColumnToStartFrom,
                                                                           out double MinOfNonZeroValues, out double MaxOfNonZeroValues)
        {
            MinOfNonZeroValues = Double.PositiveInfinity; // +Inf at the beginning
            MaxOfNonZeroValues = Double.NegativeInfinity; // -Inf at the beginning

            for (int j = ColumnToStartFrom; j < NbColumn; j++)
            {
                double CurrentScannedElement = InputHashlist.get(RowToScan, j);
                if (CurrentScannedElement != 0)
                {
                    if (CurrentScannedElement < MinOfNonZeroValues)
                    {
                        MinOfNonZeroValues = CurrentScannedElement;
                    }
                    if (CurrentScannedElement > MaxOfNonZeroValues)
                    {
                        MaxOfNonZeroValues = CurrentScannedElement;
                    }
                }
            }
        }
        public static void ReturnMinAndMaxOfNonZeroElementsInARowOfAMatrix(Matrix<double> InputMatrix, int RowToScan, int ColumnToStartFrom,
                                                                           out double MinOfNonZeroValues, out double MaxOfNonZeroValues)
        {
            MinOfNonZeroValues = Double.PositiveInfinity; // +Inf at the beginning
            MaxOfNonZeroValues = Double.NegativeInfinity; // -Inf at the beginning

            int LastColumnToScan = InputMatrix.ColumnCount;
            for (int j = ColumnToStartFrom; j < LastColumnToScan; j++)
            {
                double CurrentScannedElement = InputMatrix[RowToScan, j];
                if (CurrentScannedElement != 0)
                {
                    if (CurrentScannedElement < MinOfNonZeroValues)
                    {
                        MinOfNonZeroValues = CurrentScannedElement;
                    }
                    if (CurrentScannedElement > MaxOfNonZeroValues)
                    {
                        MaxOfNonZeroValues = CurrentScannedElement;
                    }
                }
            }
        }


        public static string dehexify_password(string input_line)
        {
            string dehex_string;
            if (input_line.StartsWith("$HEX[") && input_line.EndsWith("]") && (input_line.Count() % 2 == 0))
            {
                string HexText = input_line.Substring(5, input_line.Length - 6);
                byte[] bb = Enumerable.Range(0, HexText.Length).Where(x => x % 2 == 0)
                    .Select(x => Convert.ToByte(HexText.Substring(x, 2), 16)).ToArray();
                dehex_string = System.Text.Encoding.ASCII.GetString(bb);
            }
            else dehex_string = input_line;
            return dehex_string;
        }

        // Training the Adaptive memory "matrix" with a hashlist


        public static void TrainMMmem_Hashlist_Ascii(string PasswordsListTxtFileName, int MemSize,
                                        string output_path, Action<string> callback_console)
        {

            SMtrx1D M = new SMtrx1D();
            //Console.WriteLine(M.ToString());
            string console_text = "";
            if (!File.Exists(PasswordsListTxtFileName))
            {
                console_text = "Error: " + PasswordsListTxtFileName  + " not found!";
                callback_console(console_text);
            }
            else
            {
                const int TOFFSET = 28;  // Last printable ascii char is 126(0x7F) which becomes 126 - 28 = 98.
                                         // First printable ascii is 32(0x20) and becomes 32 - 28 = 4.
                const int PSTART = 1; // Beginning of password is 1. We can refer to it with the symbol ^ (just a convention)
                const int PEND = 99; // End of password is 99. We can refer to it with the symbol $ (just a convention)

                // Pre-calculation of the total number of passwords (useful for estimating the time to end processing)
                //int NumberOfAllPasswords = File.ReadLines(PasswordsListTxtFileName).Count();
                //Variant that is supposed to work faster
                int NumberOfAllPasswords = 0;
                using (var reader = File.OpenText(PasswordsListTxtFileName))
                {
                    while (reader.ReadLine() != null)
                    {
                        NumberOfAllPasswords++;
                    }
                }

                console_text = "[-] Found "+ NumberOfAllPasswords + " lines in the password list, before processing.";
                callback_console(console_text);

                if (NumberOfAllPasswords == 0)
                {
                    console_text = "[!] Error: " + PasswordsListTxtFileName +" does not contain any password!";
                    callback_console(console_text);
                    return;
                }

                long FileLength = new System.IO.FileInfo(PasswordsListTxtFileName).Length;
                StreamReader myStreamOfPasswords = File.OpenText(PasswordsListTxtFileName);
                string CurrentReadPassword; // In Matlab's code this variable is simply called p
                int TotalBytesUntilNow = 0;
                //int TotalPasswordsUntilNow = 0;
                int CurrentNumberOfReadPasswords = 0; // A read password can be acceptable or incorrect
                int CurrentNumberOfGoodPasswords = 0; // A good(accepted) password satisfies the suitable requirements
                int[] ngrams = new int[MemSize];

                // Initialize TextProgressInfo engine V4
                int NumberOfIterationsWithoutUpdatingTextProgressInfo = (int)Math.Ceiling((double)NumberOfAllPasswords / (double)200);
                Stopwatch StopWatchObj = JRC_TextProgressInfoV4_Init();
                JRC_TextProgressInfoV4_Show(0, //int CurrentValueOfIteration
                                            NumberOfAllPasswords, //int EndValueOfIteration
                                            StopWatchObj, // Stopwatch StopWatchVariableUsedInInit
                                            1, // int TimeForPauseInSeconds
                                            NumberOfIterationsWithoutUpdatingTextProgressInfo,// int NumberOfIterationsWithoutUpdatingText
                                            callback_console); //callback function for console display

                while ((CurrentReadPassword = myStreamOfPasswords.ReadLine()) != null)
                {
                    CurrentNumberOfReadPasswords++;
                    //CurrentReadPassword = dehexify_password(CurrentReadPassword);
                    int LengthOfCurrentPassword = CurrentReadPassword.Length;
                    // first: checking if the password contains special char or is empty
                    if (PasswordIsCorrectAndNotEmpty(CurrentReadPassword))
                    {
                        CurrentNumberOfGoodPasswords++;
                        TotalBytesUntilNow = TotalBytesUntilNow + LengthOfCurrentPassword + 1;

                        // Generate a list to store the memsize differents ngrams. 
                        // It is initalised by ones corresponding to the value PSTART
                        int i, j;
                        for (i = 0; i < ngrams.Length; i++)
                        {
                            ngrams[i] = PSTART; // PSTART is 1
                        }

                        int next_char;
                        for (i = 0; i <= LengthOfCurrentPassword; i++)
                        {
                            // Get num value of next character(or terminator of password)
                            if (i == LengthOfCurrentPassword)
                            {
                                next_char = PEND;
                            }
                            else
                            {
                                next_char = ((int)(CurrentReadPassword[i])) - TOFFSET; // next letter in ASCII code -28
                            }

                            for (j = 1; j <= Math.Min(i + 1, MemSize); j++) // the min corresponds to the number of ngram to consider this round
                            {
                                // Let's increase the corresponding transition by 1
                                M.inc(ngrams[j - 1] - 1, next_char - 1); //NOTE: -1 is present 3 times because indices in C# are 0 based, while in Matlab 1 based
                                // Let's increase the total number of transition of the ngram by 1
                                M.inc(ngrams[j - 1] - 1, 0); //NOTE: -1 is present 2 times because indices in C# are 0 based, while in Matlab 1 based
                            }

                            for (j = 1; j <= MemSize; j++)
                            {
                                // Let's translate Matlab instruction: ngrams(j) = mod(ngrams(j) * 100 + next_char, 10 ^ (2 * j)); % the modulus is only keeping the needed number of transitions
                                ngrams[j - 1] = ((int)(ngrams[j - 1] * 100 + next_char)) % ((int)(Math.Pow(10, (2 * j)))); // the modulus is only keeping the needed number of transitions
                            }

                        } //end for i
                    }
                    else
                    {
                        //Console.WriteLine("Current password ({0}) at line {1} is incorrect", CurrentReadPassword, NumberOfReadLinesInFileUntilNow);
                    }
                    JRC_TextProgressInfoV4_Show(CurrentNumberOfReadPasswords, //int CurrentValueOfIteration
                                           NumberOfAllPasswords, //int EndValueOfIteration
                                           StopWatchObj, // Stopwatch StopWatchVariableUsedInInit
                                           0, // int TimeForPauseInSeconds
                                           NumberOfIterationsWithoutUpdatingTextProgressInfo,// int NumberOfIterationsWithoutUpdatingText
                                           callback_console); //callback function for console display

                } // end while

                console_text = "Processing of passwords: completed. Accepted passwords:" + CurrentNumberOfGoodPasswords + "; Incorrect passwords: " +
                                  (CurrentNumberOfReadPasswords - CurrentNumberOfGoodPasswords);
                callback_console(console_text);

                const int ColumnToStartResearchFrom = 3; // We neglect, while searching for min and MAX, the first three columns: 0, 1, 2.
                for (int i = 0; i < Math.Pow(100, MemSize); i++)
                {
                    if (M.get(i, 0) != 0)
                    {
                        ReturnMinAndMaxOfNonZeroElementsInHashlist(M, i, 100, ColumnToStartResearchFrom, out double MinValue, out double MaxValue);
                        M.set(i, 1, MaxValue); //Second column updated (Remember: indices are 0 based)
                        M.set(i, 2, MinValue); //Third column updated (Remember: indices are 0 based)
                    }
                }

                myStreamOfPasswords.Close();

            }
            // Let's save the hashlist
            console_text = "Preparing output matrix";
            callback_console(console_text);
            //save2 is the new saving object
            M.save2(output_path);

        }  // end function TrainMMmem_Hashlist_Ascii



        public static void TrainMMmem_Hashlist_Uni1K(string PasswordsListTxtFileName, int MemSize,
                                                     string output_path, Action<string> callback_console)

        {

            SMtrx1D M = new SMtrx1D(); 

            // NOTE: the first 3 columns of M (the transition matrix) are used to:
            //      1st --> Total numbers of transitions seen during the training of a given n-gram
            //              (used to compute on the fly the probability of occurrence of a given transition)
            //
            //      2nd --> Maximum value of the row associated to an n-gram. I.e. the value corresponding to
            //              the transition (n-gram --> char) that have been the most seen during the training,
            //              without caring the char to which the transition corresponds to. It's used to compute
            //              a dynamic "bonus" to attribute to the score of a password. Storing it is useful because
            //              it does not need to be recomputed every time it's needed.
            //
            //      3rd --> Minimum value of the row associated to an n-gram. I.e. the value corresponding to
            //              the transition (n-gram --> char) that have been the least seen during the training,
            //              without caring the char to which the transition corresponds to. It's used to compute
            //              a dynamic "bonus" to attribute to the score of a password. Storing it is useful because
            //              it does not need to be recomputed every time it's needed.
            Console.WriteLine("Just created an empty Hashlist.");


            // Corresponding to Matlab's instruction: M = sparse(1000 ^ memsize, 1000);
            // PLEASE NOTE: in C#, ^ is NOT the power operator! It's a logical XOR!!!
            // PLEASE NOTE: probably M could be a int matrix, instead of double, for this algorithm


            if (!File.Exists(PasswordsListTxtFileName))
            {
                Console.WriteLine("Error: {0} not found!", PasswordsListTxtFileName);
            }
            else
            {
                const int TOFFSET = 28;  // Last printable ascii char is 126(0x7F) which becomes 126 - 28 = 98.
                                         // First printable ascii is 32(0x20) and becomes 32 - 28 = 4.
                const int PSTART = 1; // Beginning of password is 1. We can refer to it with the symbol ^ (just a convention)
                const int PEND = 999; // End of password is 999. We can refer to it with the symbol $ (just a convention)



                // Pre-calculation of the total number of passwords (useful for estimating the time to end processing)
                int NumberOfAllPasswords = File.ReadLines(PasswordsListTxtFileName).Count();
                Console.WriteLine("Found {0} lines in the password list, before processing.", NumberOfAllPasswords);

                if (NumberOfAllPasswords == 0)
                {
                    Console.WriteLine("Error: {0} does not contain any password!", PasswordsListTxtFileName);
                    return;
                }

                long FileLength = new System.IO.FileInfo(PasswordsListTxtFileName).Length;

                StreamReader myStreamOfPasswords = File.OpenText(PasswordsListTxtFileName);

                string CurrentReadPassword; // In Matlab's code this variable is simply called p
                // int TotalBytesUntilNow = 0;
                //int TotalPasswordsUntilNow = 0;
                int CurrentNumberOfReadPasswords = 0; // A read password can be acceptable or incorrect
                int CurrentNumberOfGoodPasswords = 0; // A good(accepted) password satisfies the suitable requirements
                int CurrentNumberOfPureAsciiPasswords = 0; // PureAsciiPasswords have all characters below [128]Dec
                int[] ngrams = new int[MemSize];

                // Initialize TextProgressInfo engine V4
                int NumberOfIterationsWithoutUpdatingTextProgressInfo = (int)Math.Ceiling((double)NumberOfAllPasswords / (double)200);
                Stopwatch StopWatchObj = JRC_TextProgressInfoV4_Init();
                JRC_TextProgressInfoV4_Show(0, //int CurrentValueOfIteration
                                            NumberOfAllPasswords, //int EndValueOfIteration
                                            StopWatchObj, // Stopwatch StopWatchVariableUsedInInit
                                            1, // int TimeForPauseInSeconds
                                            NumberOfIterationsWithoutUpdatingTextProgressInfo,// int NumberOfIterationsWithoutUpdatingText
                                            callback_console); //callback function for console display

                while ((CurrentReadPassword = myStreamOfPasswords.ReadLine()) != null)
                {
                    CurrentNumberOfReadPasswords++;
                    //CurrentReadPassword = dehexify_password(CurrentReadPassword);

                    // Console.WriteLine("CurrentNumberOfReadPasswords = {0}", CurrentNumberOfReadPasswords);
                    int LengthOfCurrentPassword = CurrentReadPassword.Length;
                    // first: checking if the password contains special char or is empty
                    if (PasswordIsCorrectAndNotEmpty(CurrentReadPassword))
                    {
                        CurrentNumberOfGoodPasswords++;

                        if (IsASCII(CurrentReadPassword))
                        {
                            CurrentNumberOfPureAsciiPasswords++;
                        }

                        // Generate a list to store the memsize differents ngrams. 
                        // It is initalised by ones corresponding to the value PSTART
                        int i, j;
                        for (i = 0; i < ngrams.Length; i++)
                        {
                            ngrams[i] = PSTART; // PSTART is 1
                        }

                        int next_char;
                        for (i = 0; i <= LengthOfCurrentPassword; i++)
                        {
                            // Get num value of next character(or terminator of password)
                            if (i == LengthOfCurrentPassword)
                            {
                                next_char = PEND;
                            }
                            else
                            {
                                next_char = ((int)(CurrentReadPassword[i])) - TOFFSET; // next letter in ASCII code -26
                            }

                            for (j = 1; j <= Math.Min(i + 1, MemSize); j++) // the min corresponds to the number of ngram to consider this round
                            {
                                // Let's increase the corresponding transition by 1
                                M.inc(ngrams[j - 1] - 1, next_char - 1); //NOTE: -1 is present 3 times because indices in C# are 0 based, while in Matlab 1 based
                                // Let's increase the total number of transition of the ngram by 1
                                M.inc(ngrams[j - 1] - 1, 0); //NOTE: -1 is present 2 times because indices in C# are 0 based, while in Matlab 1 based
                            }

                            for (j = 1; j <= MemSize; j++)
                            {
                                // Let's translate Matlab instruction: ngrams(j) = mod(ngrams(j) * 100 + next_char, 10 ^ (2 * j)); % the modulus is only keeping the needed number of transitions
                                ngrams[j - 1] = (int)(((double)ngrams[j - 1] * 1000.0 + next_char) % ((int)(Math.Pow(1000, j)))); // the modulus is only keeping the needed number of transitions
                            }

                        } //end for i

                    }
                    else
                    {
                        //Console.WriteLine("Current password ({0}) at line {1} is incorrect", CurrentReadPassword, NumberOfReadLinesInFileUntilNow);
                    }
                    JRC_TextProgressInfoV4_Show(CurrentNumberOfReadPasswords, //int CurrentValueOfIteration
                       NumberOfAllPasswords, //int EndValueOfIteration
                       StopWatchObj, // Stopwatch StopWatchVariableUsedInInit
                       0, // int TimeForPauseInSeconds
                       NumberOfIterationsWithoutUpdatingTextProgressInfo,// int NumberOfIterationsWithoutUpdatingText
                       callback_console); //callback function for console display
                } // end while

                Console.WriteLine("Processing of passwords: completed. Accepted passwords: {0}; Incorrect passwords: {1}", +
                                  CurrentNumberOfGoodPasswords, (CurrentNumberOfReadPasswords - CurrentNumberOfGoodPasswords));

                Console.WriteLine("Evaluating min and max of non zero elements... ");

                const int ColumnToStartResearchFrom = 3; // We neglect, while searching for min and MAX, the first three columns: 0, 1, 2.
                for (int i = 0; i < Math.Pow(1000, MemSize); i++)
                {
                    if (M.get(i, 0) != 0)
                    {
                        ReturnMinAndMaxOfNonZeroElementsInHashlist(M, i, 1000, ColumnToStartResearchFrom, out double MinValue, out double MaxValue);
                        M.set(i, 1, MaxValue); //Second column updated (Remember: indices are 0 based)
                        M.set(i, 2, MinValue); //Third column updated (Remember: indices are 0 based)
                    }
                }

            }
            // Let's save the matrix
            M.save(output_path);

            //return M; 
        } //end TrainMMmem_Hashlist_Uni1K()

        public static void ShowToScreenAMatchCollection(MatchCollection MatchCollectionToShowToScreen)
        {
            int i = 0;
            foreach (Match CurrentMatch in MatchCollectionToShowToScreen)
            {
                //Console.WriteLine("Match n.{0} - Length = {1} - Index = {2}", i, CurrentMatch.Value.Length, CurrentMatch.Index); // NOTE: here index is 0 based, while in Matlab is 1-based
                Console.WriteLine("Case {0} out of {1} - Value: {2} - Length: {3}", i, MatchCollectionToShowToScreen.Count, CurrentMatch.Value, CurrentMatch.Value.Length);
                i++;
            }

        }

        public static void FindTokensForTraining(string InputString,
                              out int lengthPasswordTokens,
                              out int[] vectLengthTokensNum,
                              out int[] vectLengthTokensAlfa,
                              out int[] vectLengthTokensSpecial,
                              out int[] vectInds_Final,
                              out MatchCollection SetOfNum_Matches,
                              out MatchCollection SetOfAlfa_Matches,
                              out MatchCollection SetOfSpe_Matches)
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


            SetOfNum_Matches = Regex.Matches(InputString, numExpr);

            //Console.WriteLine("Using RegEx, there are '{0}' found matches for Num.", SetOfNum_Matches.Count);
            numberOfNum = SetOfNum_Matches.Count;
            vectLengthTokensNum = new int[numberOfNum];
            vectStartIndicesNum = new int[numberOfNum];
            i = 0;
            foreach (Match CurrentMatch in SetOfNum_Matches)
            {
                //Console.WriteLine("----New Match----");
                //Console.WriteLine("'{0}' found at index {1}.", CurrentMatch.Value, CurrentMatch.Index);
                //Console.WriteLine("'{0}' is its length.", CurrentMatch.Value.Length);
                vectLengthTokensNum[i] = CurrentMatch.Value.Length;
                vectStartIndicesNum[i] = CurrentMatch.Index; // NOTE: here index is 0 based, while in Matlab is 1-based
                i++;
                /*
                GroupCollection GroupsInCurrentMatch = CurrentMatch.Groups;
                //Console.WriteLine(" Word value in current match's group: '{0}'", GroupsInCurrentMatch["word"].Value);
                Console.WriteLine(" Count value in current match's group: '{0}'", GroupsInCurrentMatch.Count);

                for (int i = 0; i < GroupsInCurrentMatch.Count; i++)
                {
                    if (i == 0)
                    {
                        Console.WriteLine("New 'for loop'");
                    }
                    Console.WriteLine("Match found at position {0}", GroupsInCurrentMatch[i].Index);
                }
                */
            }



            SetOfAlfa_Matches = Regex.Matches(InputString, alfaExpr);
            //Console.WriteLine("Using RegEx, there are '{0}' found matches for Alfa.", SetOfAlfa_Matches.Count);
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



            SetOfSpe_Matches = Regex.Matches(InputString, specialExpr);
            //Console.WriteLine("Using RegEx, there are '{0}' found matches for Spe.", SetOfSpe_Matches.Count);
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
        } //End function FindTokensForTraining


        public static bool CheckIfEveryByteOfArrayIsInsideARange(byte[] ArrayOfBytesToCheck, byte LowerLimit, byte UpperLimit)
        {
            int i;
            bool BoolValueToReturn = true;
            for (i = 0; i < ArrayOfBytesToCheck.Length; i++)
            {
                if ((ArrayOfBytesToCheck[i] < LowerLimit) || (ArrayOfBytesToCheck[i] > UpperLimit))
                {
                    BoolValueToReturn = false;
                    break;
                }
            }
            return BoolValueToReturn;
        }

        public static bool CheckIfEveryIntOfArrayIsInsideARange(int[] ArrayOfIntegersToCheck, int LowerLimit, int UpperLimit)
        {
            int i;
            bool BoolValueToReturn = true;
            for (i = 0; i < ArrayOfIntegersToCheck.Length; i++)
            {
                if ((ArrayOfIntegersToCheck[i] < LowerLimit) || (ArrayOfIntegersToCheck[i] > UpperLimit))
                {
                    BoolValueToReturn = false;
                    break;
                }
            }
            return BoolValueToReturn;
        }



        public static bool CheckIfEveryCharOfStringIsInsideAUnicode1KRange(string StringToCheck, int LowerLimit, int UpperLimit)
        {
            int i;
            bool BoolValueToReturn = true;
            for (i = 0; i < StringToCheck.Length; i++)
            {
                if (   ((int)(StringToCheck[i]) < LowerLimit)    ||    (((int)StringToCheck[i]) > UpperLimit)   )
                {
                    BoolValueToReturn = false;
                    break;
                }
            }
            return BoolValueToReturn;
        }


        public static int ReturnTheMaxValueInOneArrayOrZeroIfTheArrayIsEmpty(int[] InputArray)
        {
            if (InputArray.Length == 0) return (int)0;
            else
            {
                //If I am here, then InputArray has at least one element, so...
                int CurrentMax = InputArray[0];  // NOT: int CurrentMax = int.MinValue; // -Inf at the beginning
                for (int i = 1; i < InputArray.Length; i++) //NOTE: i does not start from 0...
                {
                    if (InputArray[i] > CurrentMax)
                    {
                        CurrentMax = InputArray[i];
                    }
                }
                return CurrentMax;
            }
        }

        public static void IncrementTheContentOfACell_InMLDoubleVector(MLDouble MLDoubleVectorToModify, int IndexOfTheElementToIncrement)
        {
            MLDoubleVectorToModify.Set(MLDoubleVectorToModify.Get(IndexOfTheElementToIncrement) + 1, IndexOfTheElementToIncrement);
        }

        public static void IncrementTheContentOfACell_InMLDoubleMatrix(MLDouble MLDoubleMatrixToModify, int RowOfTheElementToIncrement, int ColumnOfTheElementToIncrement)
        {
            MLDoubleMatrixToModify.Set(MLDoubleMatrixToModify.Get(RowOfTheElementToIncrement, ColumnOfTheElementToIncrement) + 1, RowOfTheElementToIncrement, ColumnOfTheElementToIncrement);
        }

        public static void IncrementTheContentOfACell_InMLDoubleTensor(MLDouble MLDoubleTensorToModify, int RowOfTheElementToIncrement, int ColumnOfTheElementToIncrement, int SliceOfTheElementToIncrement)
        {
            // In this code we use double indexing to access a tensor in C#. After the instructions, there is a valid alternative if we wanted single indexing to access a tensor.
            int NumberOfColumnsInTensor = MLDoubleTensorToModify.Dimensions[1]; // 1 because 0 is rows, 1 is columns, 2 is slices
            double OldValue = MLDoubleTensorToModify.Get(RowOfTheElementToIncrement, ColumnOfTheElementToIncrement + SliceOfTheElementToIncrement * NumberOfColumnsInTensor);
            MLDoubleTensorToModify.Set(OldValue + 1, RowOfTheElementToIncrement, ColumnOfTheElementToIncrement + SliceOfTheElementToIncrement * NumberOfColumnsInTensor);
            // NOTE: If we wanted to use single indexing to access a tensor, the rules would be the following ones:
            // R=number of total rows; C=number of total columns; S=number of total slices;
            // row=row to access; col=column to access; sli=slice to access; {All those indices must be in C# syntax, i.e. 0-based (not 1 based as in Matlab)}
            // SingleIndex = row + R*col + R*C*sli;

            // If we had row, col and sli that are 1-based, then each of the three must be submitted to the operator --. In other words: SingleIndex = row-1 + R*(col-1) + R*C*(sli-1);
        }

        public static double SumAllElements_InMLDoubleVector(MLDouble MLDoubleInputVector)
        {
            double Sum = 0.0;
            for (int i = 0; i < MLDoubleInputVector.N; i++)
            {
                Sum += MLDoubleInputVector.Get(i);
            }
            return Sum;
        }

        public static void DivideEachElementByADoubleConstant_InMLDoubleVector(MLDouble MLDoubleInputVector, double ConstantForDivision)
        {
            double OldValue;
            for (int i = 0; i < MLDoubleInputVector.N; i++)
            {
                OldValue = MLDoubleInputVector.Get(i);
                MLDoubleInputVector.Set(OldValue / ConstantForDivision, i);
            }
        }

        public static MLDouble SumTheElementsInEveryRow_InMLDoubleMatrix(MLDouble MLDoubleInputMatrix)
        {
            double CurrentValue;
            int[] DimOfOutputVector = new int[] { MLDoubleInputMatrix.M, 1 };
            MLDouble OutputVector = new MLDouble("VectorOfSumsAlongEveryRow", DimOfOutputVector);

            for (int i = 0; i < MLDoubleInputMatrix.M; i++)
            {
                CurrentValue = 0.0;
                for (int j = 0; j < MLDoubleInputMatrix.N; j++)
                {
                    CurrentValue += MLDoubleInputMatrix.Get(i, j);
                }
                OutputVector.Set(CurrentValue, i);
            }
            return OutputVector;
        }

        public static MLDouble SumTheElementsInEveryRow_InSliceOfMLDoubleTensor(MLDouble MLDoubleInputTensor, int Slice)
        // NOTE: In this code we use double indexing to access a tensor in C#. Note that there is a valid alternative if we wanted single indexing to access a tensor.
        // Indeed, if we wanted to use single indexing to access a tensor, the rules would be the following ones:
        // R=number of total rows; C=number of total columns; S=number of total slices;
        // row=row to access; col=column to access; sli=slice to access; {All those indices must be in C# syntax, i.e. 0-based (not 1 based as in Matlab)}
        // SingleIndex = row + R*col + R*C*sli;
        // NOTE: If we had row, col and sli that are 1-based, then each of the three must be submitted to the operator --. In other words: SingleIndex = row-1 + R*(col-1) + R*C*(sli-1);
        {
            double CurrentValue;

            int NumberOfRowsInTensor = MLDoubleInputTensor.Dimensions[0]; // 1 because 0 is rows, 1 is columns, 2 is slices
            int NumberOfColumnsInTensor = MLDoubleInputTensor.Dimensions[1]; // 1 because 0 is rows, 1 is columns, 2 is slices
            int[] DimOfOutputVector = new int[] { NumberOfRowsInTensor, 1 };

            MLDouble OutputVector = new MLDouble("VectorOfSumsAlongEveryRow", DimOfOutputVector);

            for (int i = 0; i < NumberOfRowsInTensor; i++)
            {
                CurrentValue = 0.0;
                for (int j = 0; j < NumberOfColumnsInTensor; j++)
                {
                    CurrentValue += MLDoubleInputTensor.Get(i, j + Slice * NumberOfColumnsInTensor);
                }
                OutputVector.Set(CurrentValue, i);
            }
            return OutputVector;
        }


        public static void NormalizeAMatrixOfFrequenciesInProbabilities_InMLDoubleMatrix(MLDouble MLDoubleInputMatrix)
        // NOTE: If the NormalizationFactor is 0, then it is changed to -1
        {
            MLDouble VectorForNormalization = SumTheElementsInEveryRow_InMLDoubleMatrix(MLDoubleInputMatrix);
            int i, j;
            double NormalizationFactor;
            for (i = 0; i < MLDoubleInputMatrix.M; i++)
            {
                NormalizationFactor = VectorForNormalization.Get(i);
                if (NormalizationFactor == 0.0)
                {
                    NormalizationFactor = -1.0;
                }
                for (j = 0; j < MLDoubleInputMatrix.N; j++)
                {
                    MLDoubleInputMatrix.Set((MLDoubleInputMatrix.Get(i, j) / NormalizationFactor), i, j);
                }
            }

        }


        public static void NormalizeAMatrixOfFrequenciesInProbabilities_InSliceOfMLDoubleTensor(MLDouble MLDoubleInputTensor, int Slice)
        // NOTE: If the NormalizationFactor is 0, then it is changed to -1
        // NOTE: In this code we use double indexing to access a tensor in C#. Note that there is a valid alternative if we wanted single indexing to access a tensor.
        // Indeed, if we wanted to use single indexing to access a tensor, the rules would be the following ones:
        // R=number of total rows; C=number of total columns; S=number of total slices;
        // row=row to access; col=column to access; sli=slice to access; {All those indices must be in C# syntax, i.e. 0-based (not 1 based as in Matlab)}
        // SingleIndex = row + R*col + R*C*sli;
        // NOTE: If we had row, col and sli that are 1-based, then each of the three must be submitted to the operator --. In other words: SingleIndex = row-1 + R*(col-1) + R*C*(sli-1);

        {
            MLDouble VectorForNormalization = SumTheElementsInEveryRow_InSliceOfMLDoubleTensor(MLDoubleInputTensor, Slice);
            int NumberOfRowsInTensor = MLDoubleInputTensor.Dimensions[0]; // 1 because 0 is rows, 1 is columns, 2 is slices
            int NumberOfColumnsInTensor = MLDoubleInputTensor.Dimensions[1]; // 1 because 0 is rows, 1 is columns, 2 is slices

            int i, j;
            double NormalizationFactor;
            for (i = 0; i < NumberOfRowsInTensor; i++)
            {
                NormalizationFactor = VectorForNormalization.Get(i);
                if (NormalizationFactor == 0.0)
                {
                    NormalizationFactor = -1.0;
                }
                for (j = 0; j < NumberOfColumnsInTensor; j++)
                {
                    MLDoubleInputTensor.Set((MLDoubleInputTensor.Get(i, j + Slice * NumberOfColumnsInTensor) / NormalizationFactor), i, j + Slice * NumberOfColumnsInTensor);
                }
            }

        }

        public static int JRC_CountLinesInATxtFile(string TxtFileName)
        // Read an entire text file just to count the number of its rows (then, the file is closed).
        {
            return File.ReadLines(TxtFileName).Count();
        }


        public static Stopwatch JRC_TextProgressInfoV4_Init()
        // Initialize data needed to process TextProgressInfo
        {
            Stopwatch stw = Stopwatch.StartNew();
            // The above instruction replaces the two following ones:
            //      Stopwatch stw = new.StopWatch();
            //      stw.Start();
            return stw;
        }

        public static void JRC_TextProgressInfoV4_Show(int CurrentValueOfIteration,
                                                      int EndValueOfIteration,
                                                      Stopwatch StopWatchVariableUsedInInit,
                                                      int TimeForPauseInSeconds,
                                                      int NumberOfIterationsWithoutUpdatingText,
                                                      Action<string> callback_console)
        // Show TextProgressInfo data, after initialization. StopWatchVariableUsedInInit is the variable returned by
        // JRC_TextProgressInfoV4_Init(). NumberOfBlanksInPreviousCall is 0 when JRC_TextProgressInfoV4_Show is called
        // for the first time; in the following calls, the new NumberOfBlanksInPreviousCall is the value returned by
        // JRC_TextProgressInfoV4_Show in the previous call.
        {
            double tRemainingInSeconds, tElapsedInSeconds;
            double SpeedOfProcessing;
            TimeSpan tElapsedInTimeSpan;
            string StringToPrint;
            //string BackSpaceString="";            
            if (CurrentValueOfIteration % NumberOfIterationsWithoutUpdatingText == 0)
            {
                tElapsedInTimeSpan = StopWatchVariableUsedInInit.Elapsed;
                tElapsedInSeconds = StopWatchVariableUsedInInit.ElapsedMilliseconds / 1000;

                if (CurrentValueOfIteration > 0)
                {
                    tRemainingInSeconds = tElapsedInSeconds * ((double)((double)EndValueOfIteration - (double)CurrentValueOfIteration) / (double) CurrentValueOfIteration);
                    if (tElapsedInSeconds>0)
                    {
                        //SpeedOfProcessing = Math.Round(CurrentValueOfIteration / tElapsedInSeconds);
                        SpeedOfProcessing = CurrentValueOfIteration / tElapsedInSeconds;
                    }
                    else
                    {
                        SpeedOfProcessing = 0.0;
                    }
                }
                else
                {
                    tRemainingInSeconds = 0.0;
                    SpeedOfProcessing = 0.0;
                }
                TimeSpan tRemainingInTimeSpan = TimeSpan.FromSeconds(tRemainingInSeconds);
                int ProgressPercantage = (int) (100.0 * (double) CurrentValueOfIteration / (double) EndValueOfIteration);

                int TotalNumberOfHoursInTimeRemaining = tRemainingInTimeSpan.Days * 24 + tRemainingInTimeSpan.Hours;
                int TotalNumberOfHoursInTimeElapsed = tElapsedInTimeSpan.Days * 24 + tElapsedInTimeSpan.Hours;

                StringToPrint = "[*] Progress: " + ProgressPercantage.ToString() + "%" +
                                       "; Iteration: " + CurrentValueOfIteration +
                                       " out of " + EndValueOfIteration +
                                       "; Elapsed time: " +
                                       string.Format("{0}:{1:00}:{2:00}", TotalNumberOfHoursInTimeElapsed, tElapsedInTimeSpan.Minutes, tElapsedInTimeSpan.Seconds) +
                                       "; Remaining time: " + string.Format("{0}:{1:00}:{2:00}", TotalNumberOfHoursInTimeRemaining, tRemainingInTimeSpan.Minutes, tRemainingInTimeSpan.Seconds);  // YES: tRemainingInTimeSpan.Seconds
                if (SpeedOfProcessing>=1)
                {
                    double RoundedSpeedOfProcessing = Math.Round(SpeedOfProcessing, 1);
                    StringToPrint += "; Speed: " + RoundedSpeedOfProcessing.ToString() + " Iter/Sec";
                }
                else
                {
                    double RoundedInverseOfSpeed = Math.Round( 1.0 / (SpeedOfProcessing + double.Epsilon), 1);
                    StringToPrint += "; Speed: " + RoundedInverseOfSpeed.ToString() + " Sec/Iter";
                }
                
                callback_console(StringToPrint);

                if (TimeForPauseInSeconds > 0) System.Threading.Thread.Sleep(TimeForPauseInSeconds * 1000); //Sleep expects milliseconds

                if (CurrentValueOfIteration + NumberOfIterationsWithoutUpdatingText > EndValueOfIteration)
                {
                    // This is the latest iteration --> Let's print final information only

                    tElapsedInTimeSpan = StopWatchVariableUsedInInit.Elapsed;
                    tElapsedInSeconds = StopWatchVariableUsedInInit.ElapsedMilliseconds / 1000;
                    
                    TotalNumberOfHoursInTimeElapsed = tElapsedInTimeSpan.Days * 24 + tElapsedInTimeSpan.Hours;


                    StringToPrint = "[-] Final information -- Total iterations: " + EndValueOfIteration + " - Elapsed time: " +
                                           string.Format("{0}:{1:00}:{2:00}", TotalNumberOfHoursInTimeElapsed, tElapsedInTimeSpan.Minutes, tElapsedInTimeSpan.Seconds) +
                                           " - Mean speed: " + Math.Round(SpeedOfProcessing, 2).ToString() + " Iterations/Sec";
                    callback_console(StringToPrint);
                    StopWatchVariableUsedInInit.Stop();
                }
            } // end if (CurrentValueOfIteration % NumberOfIterationsWithoutUpdatingText == 0)
        } // end function

        public static void CFHMM_trainTotalsAndProbs_V2(string PasswordsDictionaryTxtFileName, int TypeOfSupportedCharset,
                                                        string String_PrefixForOutputFile, Action<string> callback_console)
        // This function is the translation of the merging of two Matlab functions: CFHMM_trainTotals.m and CFHMM_totals2probs.m
        // Version 2  because now it supports 2 different character sets, according to the following variable:
        // TypeOfSupportedCharset: 0 = ASCII; 1 = Uni1K (Unicode, first 1024 characters)
        {
            int CodePointInitial;
            int CodePointFinal;

            if (TypeOfSupportedCharset == 0)
            {
                // ASCII charset chosen
                CodePointInitial = 32; // Use 32 to include space
                CodePointFinal = 126;
                // NOTE: 126 - 33 + 1 = 94
            }
            else
            {
                // Uni1K charset chosen
                CodePointInitial = 32; // Use 32 includes space char
                CodePointFinal = 1023;
                // NOTE: 1023-32+1 = 992
            }
            int CodePointTotal = CodePointFinal - CodePointInitial + 1;
            int CodePointOffset = CodePointInitial - 1;


            // Maximum length of each token class
            // In order to train these parameters automatically and not fixed them
            // already here, in this function I would make them very big, for instance:
            const int maxLengthNumTokens = 12; // make it 20
            const int maxLengthAlfaTokens = 20; // make it 30
            const int maxLengthSpecialTokens = 6;// make it 15
            // NOTE 1: if we change any of the 3 values above, then we'll have to change also the variable matTokens below.
            // NOTE 2: in the function CFHMM_totals2probs I would only take into account
            // those token lengths that have a probability of occurrence higher than
            // 0.1%

            int tokensTotal = maxLengthNumTokens + maxLengthAlfaTokens + maxLengthSpecialTokens;

            int maxLengthAllTokens = Math.Max(maxLengthNumTokens, Math.Max(maxLengthAlfaTokens, maxLengthSpecialTokens)); // Translate Matlab code: maxLengthAllTokens=max([maxLengthNumTokens maxLengthAlfaTokens maxLengthSpecialTokens]);

            // Let's translate the following Matlab code:
            /* matTokens=['N01' ; 'N02' ; 'N03' ; 'N04' ; 'N05' ; 'N06' ; 'N07' ; 'N08' ; 'N09' ; 'N10' ; 'N11' ; 'N12' ; ...
                       'A01' ; 'A02' ; 'A03' ; 'A04' ; 'A05' ; 'A06' ; 'A07' ; 'A08' ; 'A09' ; 'A10' ; 'A11' ; 'A12' ; 'A13' ; 'A14' ; 'A15' ; 'A16' ; 'A17' ; 'A18' ; 'A19' ; 'A20' ; ...
                       'S01' ; 'S02' ; 'S03' ; 'S04' ; 'S05' ; 'S06'];
            */

            // NOTE: this matrix will have to be changed depending on the high values selected above for the maximum length of each token class
            string[] matTokens = new string[] { "N01", "N02", "N03", "N04", "N05", "N06", "N07", "N08", "N09", "N10", "N11", "N12",
                                                "A01", "A02", "A03", "A04", "A05", "A06", "A07", "A08", "A09", "A10", "A11", "A12", "A13", "A14", "A15", "A16", "A17", "A18", "A19", "A20",
                                                "S01", "S02", "S03", "S04", "S05", "S06" };

            // Maximum and minimum password length in terms of TOKENS
            // As before, in order to train the maximum length automatically, in the
            // current function I would set it to a very hight value, for instance:
            const int lengthMax = 15; // make it 25
            const int lengthMin = 1;
            // Then, in the function CFHMM_totals2probs I would only take into account
            // those password lengths that have a probability of occurrence higher than
            // 0.1 %

            // Here we define the Matlab cell array. I prefer to build it in C# using a vector of MLDouble.
            // NOTE 1: Every element of the cell array contains one TENSOR!
            // NOTE 2: The first element of the cell array contains a tensor with one slice; in other words: a matrix.
            MLDouble[] cellProb_Trans_Grammar = new MLDouble[lengthMax]; // cellProb_Trans_Grammar will contain matProb_Trans_Grammar1, matProb_Trans_Grammar2, ..., matProb_Trans_Grammar15
            for (int cont = lengthMin; cont <= lengthMax; cont++)
            {
                // Init 3D double array (tokensTotal x tokensTotal x cont elements). For example: 38 x 38 x cont
                // NOTE: Automatically all the values in each cell are set to zero.
                int[] dims = new int[] { tokensTotal, tokensTotal, cont }; // Set number of row, columns and slices
                MLDouble array3Dim = new MLDouble("matProb_Trans_Grammar" + cont.ToString(), dims);
                cellProb_Trans_Grammar[cont - 1] = array3Dim;
            }

            int[] SizeOfMatrix = new int[] { lengthMax, tokensTotal };
            MLDouble matProb_First_Grammar = new MLDouble("matProb_First_Grammar", SizeOfMatrix);

            // This vector is used to check what is the usual length of passwords(measured in tokens)
            // In this function I suggest to set the maximum length of passwords very high(I suggested 25).
            // Then, in the Matlab function CFHMM_totals2probs we will use only those transition
            // matrices of passwords which length has a probability of occurrence higher than 0.1 %
            // So, this vector will help to define: Pmax
            int[] SizeOfVector = new int[] { 1, lengthMax };
            MLDouble vectProb_Length_Grammar = new MLDouble("vectProb_Length_Grammar", SizeOfVector); // vectProb_Length_Grammar = zeros(1, lengthMax);

            /*vectProb_Length_Grammar.Set(30, 0, 0);
            Console.WriteLine("Valore di indice 0: {0}", vectProb_Length_Grammar.Get(0));
            vectProb_Length_Grammar.Set(33, 0, 3);
            Console.WriteLine("Valore di indice 0: {0}", vectProb_Length_Grammar.Get(3));
            vectProb_Length_Grammar.Set(1233, 3);
            Console.WriteLine("Valore di indice 0: {0}", vectProb_Length_Grammar.Get(3));
            */

            // These vectors are used to check what is the usual length of each token class
            // Here we have set the maximum length of each token class very high(I suggested 20, 30 and 15)
            // Then, in the function CFHMM_totals2probs we can keep only those
            // tokens which length has a probability of occurrence higher than 0.1%
            // So, these vectors will help to define: lcmax, dcmax and scmax
            SizeOfVector = new int[] { 1, maxLengthNumTokens };
            MLDouble vectProb_Length_TokenNum = new MLDouble("vectProb_Length_TokenNum", SizeOfVector); // vectProb_Length_TokenNum = zeros(1, maxLengthNumTokens);
            SizeOfVector = new int[] { 1, maxLengthAlfaTokens };
            MLDouble vectProb_Length_TokenAlfa = new MLDouble("vectProb_Length_TokenAlfa", SizeOfVector); // vectProb_Length_TokenAlfa = zeros(1, maxLengthAlfaTokens);
            SizeOfVector = new int[] { 1, maxLengthSpecialTokens };
            MLDouble vectProb_Length_TokenSpecial = new MLDouble("vectProb_Length_TokenSpecial", SizeOfVector); // vectProb_Length_TokenSpecial = zeros(1, maxLengthSpecialTokens);

            // matrices for the CONTENT of the tokens
            SizeOfMatrix = new int[] { maxLengthNumTokens, CodePointTotal };
            MLDouble matProb_First_Nums = new MLDouble("matProb_First_Nums", SizeOfMatrix); // matProb_First_Nums = zeros(maxLengthNumTokens, CodePointTotal);
            SizeOfMatrix = new int[] { maxLengthAlfaTokens, CodePointTotal };
            MLDouble matProb_First_Alfa = new MLDouble("matProb_First_Alfa", SizeOfMatrix); // matProb_First_Alfa = zeros(maxLengthAlfaTokens, CodePointTotal);
            SizeOfMatrix = new int[] { maxLengthSpecialTokens, CodePointTotal };
            MLDouble matProb_First_Special = new MLDouble("matProb_First_Special", SizeOfMatrix); // matProb_First_Special = zeros(maxLengthSpecialTokens, CodePointTotal);
            // Now we have to create a tensor, because we are translating: matProb_Trans_Tokens = zeros(CodePointTotal, CodePointTotal, maxLengthAllTokens);
            int[] SizeOfTensor = new int[] { CodePointTotal, CodePointTotal, maxLengthAllTokens }; // Set number of row, columns and slices
            MLDouble matProb_Trans_Tokens = new MLDouble("matProb_Trans_Tokens", SizeOfTensor);

            // Pre-calculation of the total number of passwords (useful for estimation the time to end processing)
            int PrecognizedNumberOfPasswords = File.ReadLines(PasswordsDictionaryTxtFileName).Count();
            string text_console = "Found " + PrecognizedNumberOfPasswords + " lines in the dictionary, before processing.\n";
            callback_console(text_console);


            long FileLength = new System.IO.FileInfo(PasswordsDictionaryTxtFileName).Length;
            StreamReader myStreamOfPasswords = File.OpenText(PasswordsDictionaryTxtFileName);
            string CurrentLine; // In Matlab's code this variable is simply called line
            int NumberOfReadLinesInFile = 0;


            while ((CurrentLine = myStreamOfPasswords.ReadLine()) != null)
            {
                NumberOfReadLinesInFile++;
                //CurrentLine = dehexify_password(CurrentLine);
                int LengthOfCurrentLine = CurrentLine.Length;
                //if (PasswordIsCorrectAndNotEmpty(CurrentLine))

                int[] CurrentLineInCodingNumbers = new int[LengthOfCurrentLine];
                for (int Index = 0; Index< LengthOfCurrentLine; Index++)
                {
                    CurrentLineInCodingNumbers[Index] = CurrentLine[Index];
                }

                if (CheckIfEveryIntOfArrayIsInsideARange(CurrentLineInCodingNumbers, CodePointInitial, CodePointFinal))
                {
                    //We retrieve all the token info from the password
                    FindTokensForTraining(CurrentLine,
                                            out int lengthPwrTokens,
                                            out int[] lengthTokensNum_V, //V is for Vectors
                                            out int[] lengthTokensAlfa_V, //V is for Vectors
                                            out int[] lengthTokensSpecial_V, //V is for Vectors
                                            out int[] vectInds,
                                            out MatchCollection numTokens_MC, //MC is for MatchCollection
                                            out MatchCollection alfaTokens_MC, //MC is for MatchCollection
                                            out MatchCollection specialTokens_MC);//MC is for MatchCollection

                    int longestTokenNum = ReturnTheMaxValueInOneArrayOrZeroIfTheArrayIsEmpty(lengthTokensNum_V);
                    int longestTokenAlfa = ReturnTheMaxValueInOneArrayOrZeroIfTheArrayIsEmpty(lengthTokensAlfa_V);
                    int longestTokenSpecial = ReturnTheMaxValueInOneArrayOrZeroIfTheArrayIsEmpty(lengthTokensSpecial_V);

                    if ((lengthPwrTokens >= lengthMin) && (lengthPwrTokens <= lengthMax) &&
                         (longestTokenNum <= maxLengthNumTokens) &&
                         (longestTokenAlfa <= maxLengthAlfaTokens) &&
                         (longestTokenSpecial <= maxLengthSpecialTokens))
                    {
                        //vectProb_Length_Grammar(lengthPwr)
                        IncrementTheContentOfACell_InMLDoubleVector(vectProb_Length_Grammar, lengthPwrTokens - 1); // It translates the Matlab instruction: vectProb_Length_Grammar(lengthPwr) = vectProb_Length_Grammar(lengthPwr) + 1;

                        int firstTokenInd = vectInds[0]; // It translates the Matlab instruction: firstTokenInd=structTokens.vectInds(1);
                        IncrementTheContentOfACell_InMLDoubleMatrix(matProb_First_Grammar, lengthPwrTokens - 1, firstTokenInd - 1); // It translates the Matlab instruction: matProb_First_Grammar(lengthPwr,firstTokenInd)=matProb_First_Grammar(lengthPwr,firstTokenInd)+1;
                        int nextTokenInd = firstTokenInd;
                        int auxContNum = 1;
                        int auxContAlfa = 1;
                        int auxContSpecial = 1;

                        Match currentToken_Match;
                        string currentToken;
                        int lengthToken;

                        char firstCharacter; // NOTE: in C#, there is an implicit conversion from char to int
                        char nextCharacter; // NOTE: in C#, there is an implicit conversion from char to int
                        char currentCharacter; // NOTE: in C#, there is an implicit conversion from char to int

                        for (int contToken = 1; contToken < lengthPwrTokens; contToken++) // Translates Matlab: for contToken=1:lengthPwr-1
                        {
                            int currentTokenInd = nextTokenInd;
                            nextTokenInd = vectInds[contToken]; // Translates Matlab: nextTokenInd=structTokens.vectInds(contToken+1);

                            // Now we have to translate the following Matlab instruction:
                            // cellProb_Trans_Grammar{ lengthPwr}(currentTokenInd, nextTokenInd, contToken) = cellProb_Trans_Grammar{ lengthPwr}(currentTokenInd, nextTokenInd, contToken) + 1;
                            MLDouble CurrentTensor = cellProb_Trans_Grammar[lengthPwrTokens - 1];
                            IncrementTheContentOfACell_InMLDoubleTensor(CurrentTensor, currentTokenInd - 1, nextTokenInd - 1, contToken - 1);

                            //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                            //% we train the CONTENT of the tokens %
                            //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%

                            if (currentTokenInd <= maxLengthNumTokens) //if ( (currentTokenInd>=1) && (currentTokenInd <= maxLengthNumTokens) )
                            {
                                currentToken_Match = numTokens_MC[auxContNum - 1];
                                currentToken = currentToken_Match.ToString();
                                lengthToken = currentToken.Length;
                                firstCharacter = currentToken[0];
                                IncrementTheContentOfACell_InMLDoubleMatrix(matProb_First_Nums, lengthToken - 1, firstCharacter - CodePointOffset - 1); // Translates Matlab: matProb_First_Nums(lengthToken,firstCharacter-CodePointOffset)=matProb_First_Nums(lengthToken,firstCharacter-CodePointOffset)+1;
                                auxContNum++;
                                IncrementTheContentOfACell_InMLDoubleVector(vectProb_Length_TokenNum, lengthToken - 1); // Translates Matlab: vectProb_Length_TokenNum(lengthToken)=vectProb_Length_TokenNum(lengthToken)+1;

                            }
                            else if (currentTokenInd <= maxLengthNumTokens + maxLengthAlfaTokens) // if ((currentTokenInd >= maxLengthNumTokens + 1) && (currentTokenInd <= maxLengthNumTokens + maxLengthAlfaTokens))
                            {
                                currentToken_Match = alfaTokens_MC[auxContAlfa - 1];
                                currentToken = currentToken_Match.ToString();
                                lengthToken = currentToken.Length;
                                firstCharacter = currentToken[0];
                                IncrementTheContentOfACell_InMLDoubleMatrix(matProb_First_Alfa, lengthToken - 1, firstCharacter - CodePointOffset - 1); // Translates Matlab: matProb_First_Alfa(lengthToken,firstCharacter-CodePointOffset)=matProb_First_Alfa(lengthToken,firstCharacter-CodePointOffset)+1;
                                auxContAlfa++;
                                IncrementTheContentOfACell_InMLDoubleVector(vectProb_Length_TokenAlfa, lengthToken - 1); // Translates Matlab: vectProb_Length_TokenAlfa(lengthToken)=vectProb_Length_TokenAlfa(lengthToken)+1;
                            }
                            else // if ((currentTokenInd >= maxLengthNumTokens + maxLengthAlfaTokens + 1) && (currentTokenInd <= maxLengthNumTokens + maxLengthAlfaTokens + maxLengthSpecialTokens))
                            {
                                currentToken_Match = specialTokens_MC[auxContSpecial - 1];
                                currentToken = currentToken_Match.ToString();
                                lengthToken = currentToken.Length;
                                firstCharacter = currentToken[0];
                                IncrementTheContentOfACell_InMLDoubleMatrix(matProb_First_Special, lengthToken - 1, firstCharacter - CodePointOffset - 1); // Translates Matlab: matProb_First_Special(lengthToken,firstCharacter-CodePointOffset)=matProb_First_Special(lengthToken,firstCharacter-CodePointOffset)+1;
                                auxContSpecial++;
                                IncrementTheContentOfACell_InMLDoubleVector(vectProb_Length_TokenSpecial, lengthToken - 1); // Translates Matlab: vectProb_Length_TokenSpecial(lengthToken)=vectProb_Length_TokenSpecial(lengthToken)+1;
                            }

                            nextCharacter = firstCharacter;

                            for (int contCharacter = 1; contCharacter < lengthToken; contCharacter++) // Translates Matlab: for contCharacter=1:lengthToken-1
                            {
                                currentCharacter = nextCharacter;
                                nextCharacter = currentToken[contCharacter];
                                // Now we have to translate the folloqing Matlab instruction:
                                // matProb_Trans_Tokens(currentCharacter-CodePointOffset,nextCharacter-CodePointOffset,lengthToken)=matProb_Trans_Tokens(currentCharacter-CodePointOffset,nextCharacter-CodePointOffset,lengthToken)+1;
                                IncrementTheContentOfACell_InMLDoubleTensor(matProb_Trans_Tokens, currentCharacter - CodePointOffset - 1,
                                                                                                  nextCharacter - CodePointOffset - 1,
                                                                                                  lengthToken - 1);
                            } //end for contCharacter

                        } // end for contToken

                        //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                        //% we train the CONTENT of the LAST token
                        //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
                        Match lastToken_Match;
                        string lastToken;
                        int lastTokenInd = nextTokenInd;

                        if ((lastTokenInd >= 1) && (lastTokenInd <= maxLengthNumTokens))
                        {
                            lastToken_Match = numTokens_MC[auxContNum - 1];
                            lastToken = lastToken_Match.ToString();
                            lengthToken = lastToken.Length;
                            firstCharacter = lastToken[0];
                            IncrementTheContentOfACell_InMLDoubleMatrix(matProb_First_Nums, lengthToken - 1, firstCharacter - CodePointOffset - 1); // Translates Matlab: matProb_First_Nums(lengthToken,firstCharacter-CodePointOffset)=matProb_First_Nums(lengthToken,firstCharacter-CodePointOffset)+1;
                            auxContNum++;
                            IncrementTheContentOfACell_InMLDoubleVector(vectProb_Length_TokenNum, lengthToken - 1); // Translates Matlab: vectProb_Length_TokenNum(lengthToken)=vectProb_Length_TokenNum(lengthToken)+1;

                        }
                        else if ((lastTokenInd >= maxLengthNumTokens + 1) && (lastTokenInd <= maxLengthNumTokens + maxLengthAlfaTokens))
                        {
                            lastToken_Match = alfaTokens_MC[auxContAlfa - 1];
                            lastToken = lastToken_Match.ToString();
                            lengthToken = lastToken.Length;
                            firstCharacter = lastToken[0];
                            IncrementTheContentOfACell_InMLDoubleMatrix(matProb_First_Alfa, lengthToken - 1, firstCharacter - CodePointOffset - 1); // Translates Matlab: matProb_First_Alfa(lengthToken,firstCharacter-CodePointOffset)=matProb_First_Alfa(lengthToken,firstCharacter-CodePointOffset)+1;
                            auxContAlfa++;
                            IncrementTheContentOfACell_InMLDoubleVector(vectProb_Length_TokenAlfa, lengthToken - 1); // Translates Matlab: vectProb_Length_TokenAlfa(lengthToken)=vectProb_Length_TokenAlfa(lengthToken)+1;
                        }
                        else if ((lastTokenInd >= maxLengthNumTokens + maxLengthAlfaTokens + 1) && (lastTokenInd <= maxLengthNumTokens + maxLengthAlfaTokens + maxLengthSpecialTokens))
                        {
                            lastToken_Match = specialTokens_MC[auxContSpecial - 1];
                            lastToken = lastToken_Match.ToString();
                            lengthToken = lastToken.Length;
                            firstCharacter = lastToken[0];
                            IncrementTheContentOfACell_InMLDoubleMatrix(matProb_First_Special, lengthToken - 1, firstCharacter - CodePointOffset - 1); // Translates Matlab: matProb_First_Special(lengthToken,firstCharacter-CodePointOffset)=matProb_First_Special(lengthToken,firstCharacter-CodePointOffset)+1;
                            auxContSpecial++;
                            IncrementTheContentOfACell_InMLDoubleVector(vectProb_Length_TokenSpecial, lengthToken - 1); // Translates Matlab: vectProb_Length_TokenSpecial(lengthToken)=vectProb_Length_TokenSpecial(lengthToken)+1;
                        }
                        else
                        {
                            // The construct with an if, two elseif and one else is needed otherwise the compiler says that not every branch of the code
                            // leads to initialize some variables: firstCharacter; lengthToken; currentToken.
                            firstCharacter = '?';
                            lengthToken = 0;
                            lastToken = "";
                            callback_console("[!] Something weird happened in CFHMM_trainTotals()\n");
                        }

                        nextCharacter = firstCharacter;

                        for (int contCharacter = 1; contCharacter < lengthToken; contCharacter++) // Translates Matlab: for contCharacter=1:lengthToken-1
                        {
                            currentCharacter = nextCharacter;
                            nextCharacter = lastToken[contCharacter];
                            // Now we have to translate the folloqing Matlab instruction:
                            // matProb_Trans_Tokens(currentCharacter-CodePointOffset,nextCharacter-CodePointOffset,lengthToken)=matProb_Trans_Tokens(currentCharacter-CodePointOffset,nextCharacter-CodePointOffset,lengthToken)+1;
                            IncrementTheContentOfACell_InMLDoubleTensor(matProb_Trans_Tokens, currentCharacter - CodePointOffset - 1,
                                                                                              nextCharacter - CodePointOffset - 1,
                                                                                              lengthToken - 1);
                        } //end for contCharacter



                    } // end if ( (lengthPwrTokens >= lengthMin) && (lengthPwrTokens <= lengthMax) && ...

                } // end if (CheckIfEveryByteOfArrayIsInsideARange(CurrentLineInCodingNumbers, CodePointInitial, CodePointFinal))

            } // end while CurrentLine != NULL


            myStreamOfPasswords.Close();

            //// ------- Here we start translating CFHMM_totals2probs -------

            ///* In order to automatically train some parameters (maxLengthNumTokens, maxLengthAlfaTokens, maxLengthSpecialTokens)
            //   I would take the vectors: vectProb_Length_TokenNum, vectProb_Length_TokenAlfa, vectProb_Length_TokenSpecial
            //   and for each of them I would see what is the maximum length that has a
            //   probability of occurrence higher than 0.1 %.
            //*/

            ///* In order to automatically train the parameter lengthMax,
            //   I would take the vectors vectProb_Length_Grammar
            //   and I would see what is the maximum length that has a
            //   probability of occurrence higher than 0.1 %.
            //*/

            // We convert fom TOTAL matrices to PROBABILITY matrices

            // I have to translate the Matlab instruction: vectProb_Length_Grammar = vectProb_Length_Grammar / sum(vectProb_Length_Grammar);
            double NormalizationFactor = SumAllElements_InMLDoubleVector(vectProb_Length_Grammar);
            DivideEachElementByADoubleConstant_InMLDoubleVector(vectProb_Length_Grammar, NormalizationFactor);

            // I have to translate the Matlab instructions:
            // vectProb_FirstTotal_Grammar = sum(matProb_First_Grammar,2);
            // matProb_FirstTotal_Grammar = repmat(vectProb_FirstTotal_Grammar,1,tokensTotal); // This means that vectProb_FirstTotal_Grammar is replicated "tokensTotal" times along columns
            // matProb_First_Grammar = matProb_First_Grammar./ matProb_FirstTotal_Grammar;
            NormalizeAMatrixOfFrequenciesInProbabilities_InMLDoubleMatrix(matProb_First_Grammar);

            // Now for loop for the array of tensors cellProb_Trans_Grammar
            for (int IndexOfTensorInArray = lengthMin; IndexOfTensorInArray <= lengthMax; IndexOfTensorInArray++)
            {
                MLDouble matProb_Trans = cellProb_Trans_Grammar[IndexOfTensorInArray - 1];
                for (int IndexOfLayer = 1; IndexOfLayer <= IndexOfTensorInArray; IndexOfLayer++)
                {
                    NormalizeAMatrixOfFrequenciesInProbabilities_InSliceOfMLDoubleTensor(matProb_Trans, IndexOfLayer - 1);
                }
            }

            // I have to translate the Matlab instructions:
            // vectProb_FirstTotal_Nums = sum(matProb_First_Nums, 2);
            // matProb_FirstTotal_Nums = repmat(vectProb_FirstTotal_Nums, 1, CodePointTotal);
            // matProb_First_Nums = matProb_First_Nums./ matProb_FirstTotal_Nums;
            NormalizeAMatrixOfFrequenciesInProbabilities_InMLDoubleMatrix(matProb_First_Nums);

            // I have to translate the Matlab instructions:
            // vectProb_FirstTotal_Alfa = sum(matProb_First_Alfa, 2);
            // matProb_FirstTotal_Alfa = repmat(vectProb_FirstTotal_Alfa, 1, CodePointTotal);
            // matProb_First_Alfa = matProb_First_Alfa./ matProb_FirstTotal_Alfa;
            NormalizeAMatrixOfFrequenciesInProbabilities_InMLDoubleMatrix(matProb_First_Alfa);

            // I have to translate the Matlab instructions:
            // vectProb_FirstTotal_Special = sum(matProb_First_Special, 2);
            // matProb_FirstTotal_Special = repmat(vectProb_FirstTotal_Special, 1, CodePointTotal);
            // matProb_First_Special = matProb_First_Special./ matProb_FirstTotal_Special;
            NormalizeAMatrixOfFrequenciesInProbabilities_InMLDoubleMatrix(matProb_First_Special);

            for (int IndexOfLayer = 1; IndexOfLayer <= maxLengthAllTokens; IndexOfLayer++)
            {
                NormalizeAMatrixOfFrequenciesInProbabilities_InSliceOfMLDoubleTensor(matProb_Trans_Tokens, IndexOfLayer - 1);
            }


            // Now we can save the objects in Matlab format file. In other words, we have to translate the following Matlab instruction:
            // save CFHMM_probs cellProb_Trans_Grammar matProb_First_Grammar vectProb_Length_Grammar matProb_First_Nums matProb_First_Alfa matProb_First_Special matProb_Trans_Tokens
            List<MLArray> mlListOfMatVariablesForProbabilitiesToSave = new List<MLArray>();

            //Added by Iwen to ad the value CodePoitOffset that was missing. Adding also the other values that were mising
            int[] SizeOfScalar = new int[] { 1, 1 };
            MLDouble CodePointOffset_MatScalar = new MLDouble("CodePointOffset", SizeOfScalar);
            CodePointOffset_MatScalar.Set(CodePointOffset, 0);
            MLDouble maxLengthNumTokens_MatScalar = new MLDouble("maxLengthNumTokens", SizeOfScalar);
            maxLengthNumTokens_MatScalar.Set(maxLengthNumTokens, 0);
            MLDouble maxLengthAlfaTokens_MatScalar = new MLDouble("maxLengthAlfaTokens", SizeOfScalar);
            maxLengthAlfaTokens_MatScalar.Set(maxLengthAlfaTokens, 0);
            MLDouble maxLengthSpecialTokens_MatScalar = new MLDouble("maxLengthSpecialTokens", SizeOfScalar);
            maxLengthSpecialTokens_MatScalar.Set(maxLengthSpecialTokens, 0);
            MLDouble TypeOfSupportedCharset_MatScalar = new MLDouble("TypeOfSupportedCharset", SizeOfScalar);
            TypeOfSupportedCharset_MatScalar.Set(TypeOfSupportedCharset, 0);
            mlListOfMatVariablesForProbabilitiesToSave.Add(CodePointOffset_MatScalar);
            mlListOfMatVariablesForProbabilitiesToSave.Add(maxLengthNumTokens_MatScalar);
            mlListOfMatVariablesForProbabilitiesToSave.Add(maxLengthAlfaTokens_MatScalar);
            mlListOfMatVariablesForProbabilitiesToSave.Add(maxLengthSpecialTokens_MatScalar);
            mlListOfMatVariablesForProbabilitiesToSave.Add(TypeOfSupportedCharset_MatScalar);

            // Let's start with adding the tensors that are the content of the Matlab cell array cellProb_Trans_Grammar:
            for (int cont = lengthMin; cont <= lengthMax; cont++)
            {
                mlListOfMatVariablesForProbabilitiesToSave.Add(cellProb_Trans_Grammar[cont - 1]);
            }

            mlListOfMatVariablesForProbabilitiesToSave.Add(matProb_First_Grammar);
            mlListOfMatVariablesForProbabilitiesToSave.Add(vectProb_Length_Grammar);
            mlListOfMatVariablesForProbabilitiesToSave.Add(matProb_First_Nums);
            mlListOfMatVariablesForProbabilitiesToSave.Add(matProb_First_Alfa);
            mlListOfMatVariablesForProbabilitiesToSave.Add(matProb_First_Special);
            mlListOfMatVariablesForProbabilitiesToSave.Add(matProb_Trans_Tokens);

            MatFileWriter MatFileToWriteProbabilities = new MatFileWriter(String_PrefixForOutputFile + ".mat", mlListOfMatVariablesForProbabilitiesToSave, false); // Latest parameter is: compress?

        } // end function CFHMM_trainTotalsAndProbs_V2()




        public static void SimpleHMM_trainTotalsAndProbs_V2(string PasswordsDictionaryTxtFileName,
                                                            int TypeOfSupportedCharset, string String_PrefixForOutputFile,
                                                            Action<string> callback_console)
        // This function is the translation of the merging of two Matlab functions: simpleHMM_trainTotals.m and simpleHMM_totals2probs.m
        // Version 2  because now it supports 2 different character sets, according to the following variable:
        // TypeOfSupportedCharset: 0 = ASCII; 1 = Uni1K (Unicode, first 1024 characters)

        {
            int CodePointInitial;
            int CodePointFinal;

            if (TypeOfSupportedCharset == 0)
            {
                // ASCII charset chosen
                CodePointInitial = 32; // Use 32 to include space
                CodePointFinal = 126;
                // NOTE: 126 - 33 + 1 = 94
            }
            else
            {
                // Uni1K charset chosen
                CodePointInitial = 32; // Use 32 includes space
                CodePointFinal = 1023;
                // NOTE: 1023-32+1 = 992
            }
            int CodePointTotal = CodePointFinal - CodePointInitial + 1;
            int CodePointOffset = CodePointInitial - 1;


            // Maximum and minimum password length (in terms of CHARACTERS)
            // In order to train the lengths automatically, in the
            // current function I would set it to a extreme values, for instance:
            const int lengthMax = 24; // make it 40
            const int lengthMin = 5;  // make it 1
            // Then, in the function simpleHMM_totals2probs I would only take into account
            // those password lengths that have a probability of occurrence higher than
            // 0.1 %

            // Let's translate the Matlab instruction: matProb_Trans=zeros(CodePointTotal,CodePointTotal,lengthMax);
            int[] SizeOfTensor = new int[] { CodePointTotal, CodePointTotal, lengthMax };
            MLDouble matProb_Trans_Tensor = new MLDouble("matProb_Trans", SizeOfTensor);

            // Let's translate the Matlab instruction: matProb_First=zeros(lengthMax,CodePointTotal);
            int[] SizeOfMatrix = new int[] { lengthMax, CodePointTotal };
            MLDouble matProb_First = new MLDouble("matProb_First", SizeOfMatrix);

            // This vector is used to check what is the usual length of passwords(measured in characters)
            // In this function I suggest to set the maximum length of passwords very high(I suggested 40).
            // Then, in the function simpleHMM_totals2probs we will use only those transition
            // matrices of passwords which length has a probability of occurrence higher than 0.1 %
            // So, this vector will help to define: Lmax and Lmin
            // Let's translate the Matlab instruction: vectProb_Length = zeros(1, lengthMax);
            int[] SizeOfVector = new int[] { 1, lengthMax };
            MLDouble vectProb_Length = new MLDouble("vectProb_Length", SizeOfVector);



            // Pre-calculation of the total number of passwords (useful for estimation the time to end processing)
            int PrecognizedNumberOfPasswords = File.ReadLines(PasswordsDictionaryTxtFileName).Count();
            Console.WriteLine("Found {0} lines in the dictionary, before processing.", PrecognizedNumberOfPasswords);


            long FileLength = new System.IO.FileInfo(PasswordsDictionaryTxtFileName).Length;
            StreamReader myStreamOfPasswords = File.OpenText(PasswordsDictionaryTxtFileName);
            string CurrentLine; // In Matlab's code this variable is simply called line
            int NumberOfReadLinesInFile = 0;


            while ((CurrentLine = myStreamOfPasswords.ReadLine()) != null)
            {
                NumberOfReadLinesInFile++;
                //CurrentLine = dehexify_password(CurrentLine);

                int LengthOfCurrentLine = CurrentLine.Length;
                //if (PasswordIsCorrectAndNotEmpty(CurrentLine))

                int[] CurrentLineInCodingNumbers = new int[LengthOfCurrentLine];
                for (int Index = 0; Index < LengthOfCurrentLine; Index++)
                {
                    CurrentLineInCodingNumbers[Index] = CurrentLine[Index];
                }

                if ((CheckIfEveryIntOfArrayIsInsideARange(CurrentLineInCodingNumbers, CodePointInitial, CodePointFinal)) && (LengthOfCurrentLine >= lengthMin) && (LengthOfCurrentLine <= lengthMax))
                {

                    IncrementTheContentOfACell_InMLDoubleVector(vectProb_Length, LengthOfCurrentLine - 1); // It translates the Matlab instruction: vectProb_Length(lengthPwr)=vectProb_Length(lengthPwr)+1;

                    int firstCharacter_Int = CurrentLineInCodingNumbers[0];
                    IncrementTheContentOfACell_InMLDoubleMatrix(matProb_First, LengthOfCurrentLine - 1, firstCharacter_Int - CodePointOffset - 1); // It translates the Matlab instruction: matProb_First(lengthPwr, firstCharacter - CodePointOffset) = matProb_First(lengthPwr, firstCharacter - CodePointOffset) + 1;

                    int nextCharacter_Int = firstCharacter_Int;
                    for (int cont = 1; cont < LengthOfCurrentLine; cont++)
                    {
                        int currentCharacter_Int = nextCharacter_Int;
                        nextCharacter_Int = CurrentLineInCodingNumbers[cont]; // cont because nextCharacter_Byte it scans the second char up to the last char
                        //Let's translate the Matlab instruction: matProb_Trans(currentCharacter - CodePointOffset, nextCharacter - CodePointOffset, lengthPwr) = matProb_Trans(currentCharacter - CodePointOffset, nextCharacter - CodePointOffset, lengthPwr) + 1;
                        IncrementTheContentOfACell_InMLDoubleTensor(matProb_Trans_Tensor, currentCharacter_Int - CodePointOffset - 1, nextCharacter_Int - CodePointOffset - 1, LengthOfCurrentLine - 1);
                    }

                } // end if ((CheckIfEveryByteOfArrayIsInsideARange(CurrentLineInCodingNumbers, CodePointInitial, CodePointFinal)) && (LengthOfCurrentLine >= lengthMin) && (LengthOfCurrentLine <= lengthMax))

            } // end while

            myStreamOfPasswords.Close(); // The input file is now totally exploited, so we can close it


            int[] SizeOfScalar = new int[] { 1, 1 };
            MLDouble lengthMin_MatScalar = new MLDouble("lengthMin", SizeOfScalar);
            lengthMin_MatScalar.Set(lengthMin, 0);
            MLDouble lengthMax_MatScalar = new MLDouble("lengthMax", SizeOfScalar);
            lengthMax_MatScalar.Set(lengthMax, 0);
            MLDouble CodePointOffset_MatScalar = new MLDouble("CodePointOffset", SizeOfScalar);
            CodePointOffset_MatScalar.Set(CodePointOffset, 0);
            MLDouble TypeOfSupportedCharset_MatScalar = new MLDouble("TypeOfSupportedCharset", SizeOfScalar);
            TypeOfSupportedCharset_MatScalar.Set(TypeOfSupportedCharset, 0);


            // Now we can save the objects in Matlab format file. In other words, we have to translate the following Matlab instruction:
            // save simpleHMM_totals matProb_Trans matProb_First vectProb_Length
            List<MLArray> mlListOfMatVariablesToSave = new List<MLArray> { matProb_Trans_Tensor, matProb_First, vectProb_Length,
                                                                           lengthMin_MatScalar, lengthMax_MatScalar, CodePointOffset_MatScalar };

            // MatFileWriter MatFileToWrite = new MatFileWriter("simpleHMM_totals_Csharp.mat", mlListOfMatVariablesToSave, true); // Latest parameter is: compress?
            //MatFileWriter MatFileToWrite = new MatFileWriter(String_PrefixForOutputFile + "_JustTotals.mat", mlListOfMatVariablesToSave, false); // Latest parameter is: compress?

            // ------- Here we start translating simpleHMM_totals2probs -------
            // From the previous section of this C# function, we already have the variables: CodePointInitial, CodePointFinal, CodePointTotal, CodePointOffset, lengthMax, lengthMin
            // NOTE: In order to automatically train the password maximum length, I would take the vector vectProb_Length and I would see what is the maximum / minimum length
            //       that has a probability of occurrence higher than 0.1 %.

            // we convert from TOTAL matrices to PROBABILITY matrices
            // I have to translate the Matlab instruction: vectProb_Length = vectProb_Length/sum(vectProb_Length);
            double NormalizationFactor = SumAllElements_InMLDoubleVector(vectProb_Length);
            DivideEachElementByADoubleConstant_InMLDoubleVector(vectProb_Length, NormalizationFactor);

            NormalizeAMatrixOfFrequenciesInProbabilities_InMLDoubleMatrix(matProb_First);


            for (int IndexOfLayer = lengthMin; IndexOfLayer <= lengthMax; IndexOfLayer++)
            {
                NormalizeAMatrixOfFrequenciesInProbabilities_InSliceOfMLDoubleTensor(matProb_Trans_Tensor, IndexOfLayer - 1);
            }


            // Now we can save the objects in Matlab format file. In other words, we have to translate the following Matlab instruction:
            // save simpleHMM_probs matProb_Trans matProb_First vectProb_Length
            List<MLArray> mlListOfMatVariablesForProbabilitiesToSave = new List<MLArray> { matProb_Trans_Tensor, matProb_First, vectProb_Length,
                                                                                           lengthMin_MatScalar, lengthMax_MatScalar, CodePointOffset_MatScalar, TypeOfSupportedCharset_MatScalar };

            //MatFileWriter MatFileToWriteProbabilities = new MatFileWriter("simpleHMM_probs_Csharp.mat", mlListOfMatVariablesForProbabilitiesToSave, true); // Latest parameter is: compress?
            MatFileWriter MatFileToWriteProbabilities = new MatFileWriter(String_PrefixForOutputFile + ".mat", mlListOfMatVariablesForProbabilitiesToSave, false); // Latest parameter is: compress?


        } // end function SimpleHMM_trainTotalsAndProbs_V2()


    } // end class

} // end name space
