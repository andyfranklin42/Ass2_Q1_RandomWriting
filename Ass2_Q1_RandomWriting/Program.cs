using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ass2_Q1_RandomWriting
{
    class Program
    {
        //Random writing and Markov models of language
        private static int numGeneratedChar = 2000;

        static void Main(string[] args)
        {
            //read user inputs and return tuple
            Tuple<string, int> output = readUserInputs();

            //read file & store in Dictionary of Lists
            Dictionary<string, List<char>> fileOutput = ReadFile(output.Item1, output.Item2);

            //find initial Seed value
            string initialSeed = initialSeedValue(fileOutput);

            //write new lines of text
            string newText = writeNewLine(fileOutput, initialSeed);

            //print new text
            Console.WriteLine("New Text is :");
            Console.WriteLine(newText);

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();

        }

        
        
        private static Tuple<string, int> readUserInputs()
        {
            int markovLevel = -1;

            //read file name
            Console.WriteLine("Please enter file name :");
            string fileName = Console.ReadLine();
            //string fileName = @"c:\temp\test.txt";

            //read Markov level
            Console.WriteLine("Please enter Markov level (or type 'break' to exit):");
            while (markovLevel == -1)
            {
                string markovAttempt = Console.ReadLine();

                if (markovAttempt == "break") { break; }

                try
                {
                    markovLevel = Convert.ToInt32(markovAttempt);
                }
                catch
                {
                    Console.WriteLine("Please enter (a valid int) Markov level  (or type 'break' to exit):");
                }
            }

            return Tuple.Create(fileName, markovLevel);

        }


        //
        private static Dictionary<string, List<char>> ReadFile(string fileName, int MarkovLevel)
        {
            Dictionary<string, List<char>> finalDict = new Dictionary<string, List<char>>();
            string currKey = "";

            System.IO.StreamReader file =
                new System.IO.StreamReader(@fileName);

            while (!file.EndOfStream)
            {
                char newChar = Convert.ToChar(file.Read());

                if (Convert.ToString(newChar) != System.Environment.NewLine)
                { 
                    
                    //get to correct Markov level
                    if (currKey.Length < MarkovLevel)
                    {
                        currKey += Convert.ToString(newChar);
                    }
                    else
                    {
                        //check if key exists, if it does then add Char to the list
                        if (finalDict.ContainsKey(currKey))
                        {
                            List<char> tempList = new List<char>();
                            tempList = finalDict[currKey];
                            tempList.Add(newChar);

                        }
                        else
                        {
                            //if key doesn't exist, the create key and list with one value
                            List<char> tempList = new List<char>();
                            tempList.Add(newChar);
                            finalDict.Add(currKey, tempList);
                        }

                        currKey = currKey.Substring(1, MarkovLevel - 1) + newChar;

                    }
                }
            }

            file.Close();

            return finalDict;

        }

        private static string initialSeedValue(Dictionary<string, List<char>> fileOutput)
        {


            int maxListLen = 0;
            String initialSeed = "";

            //for now just print key followed by each in list
            foreach (var entry in fileOutput)
            {
                List<char> tempList = entry.Value;
                if (tempList.Count > maxListLen)
                {
                    maxListLen = tempList.Count;
                    initialSeed = entry.Key;
                }
            }

            //Console.WriteLine("Initial Seed is '" + initialSeed + "' with " + maxListLen + " values");
            return initialSeed;

        }

        private static string writeNewLine(Dictionary<string, List<char>> fileOutput, string initalSeed)
        {
            //create random
            Random rand = new Random();

            string seed = initalSeed;
            string newText = "";

            

            //for each random character to generate
            for (int i=0; i< numGeneratedChar; i++)
            {
                //Console.WriteLine("Seed is '" + seed + "'");
                
                //use seed to return list
                if (fileOutput.ContainsKey(seed))
                {
                    List<char> tempList = new List<char>();
                    tempList = fileOutput[seed];

                    //generate random number between 0-size of list
                    int itemInList = rand.Next(0, tempList.Count());

                    //append on to new text
                    newText += Convert.ToString(tempList[itemInList]);

                    //change seed to be (length of seed-1) + new char
                    int markovLevel = (seed.Length-1);
                    seed = seed.Substring(1, markovLevel) + Convert.ToString(tempList[itemInList]);
                }
                else
                {
                    //as the key doesn't exist, it must be the end of the file, and therefore no further
                    //char can be generated
                    return newText + " : new text returned early due to lack of key";
                }

            }

            return newText;

        }

    }
}
