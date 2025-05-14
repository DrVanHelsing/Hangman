using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using HangmanRenderer.Renderer;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hangman.Core.Game
{
    public class HangmanGame
    {
        private GallowsRenderer _renderer;

        public HangmanGame()
        {
            _renderer = new GallowsRenderer();
        }

        private string[] wordBank = { "help", "build", "computer", "current", "spectacles", "water", "chocolate", "cellphone", "physical", "table", 
                                       "different", "cables", "jacket", "television", "screen", "keyboard", "intelligent", "thermal", "dynamic", "alphabet" };

        private char[] availableLetters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

        private Random randomizer = new Random();

        private char[] correctWord;

        bool winCondition;

        private char nextGuess;

        private List<char> userGuesses = new List<char>();

        private int numLives = 6;

        private string GetWord()
        {
            return wordBank[randomizer.Next(wordBank.Length)];
        }

        private void GetGuess()
        {
            Console.SetCursorPosition(0, 15);
            Console.Write("Pick an unused letter: ");
            nextGuess = Convert.ToChar(Console.ReadLine().ToLower());
        }

        private bool CheckGuess()
        {
            if (correctWord.Contains(nextGuess))
            {
                for (int i = 0; i < correctWord.Length; i++)
                {
                    if (nextGuess == correctWord[i])
                    {
                        userGuesses[i] = correctWord[i];
                        continue;
                    }   
                }
                return true;
            }
            return false;
        }

        private static char DispBlank()
        {
            return '_';
        }

        private void UpdateWordStatus()
        {
            for (int j = 0; j < correctWord.Length; j++)
            {
                Console.SetCursorPosition(0, 13);
                Console.Write("Your current guess: ");
                Console.Write(string.Join(" ", userGuesses));
            }
        }

        private void IsWin()
        {
            winCondition = true;
            for (int i = 0; i < correctWord.Length; i++)
            {
                if ((userGuesses[i].Equals(correctWord[i])) == false)
                {
                    winCondition = false;
                    break;
                }
            }
        }

        private static string Xbox(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            return "----------------------------------";
        }

        private static string Ybox(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            return "|";
        }

        private void UpdateLetterList() 
        {
            Console.Write(Xbox(50,5));
            Console.Write(Xbox(50, 17));

            Console.SetCursorPosition(60,7);
            Console.Write("Unused Letters:");

            Console.SetCursorPosition(53,9);
            for (int letter_num = 0; letter_num < 10; letter_num++)
            {
                
                Console.Write(availableLetters[letter_num] +"  ");
            }
            Console.SetCursorPosition(53, 12);
            for (int letter_num = 10; letter_num < 20; letter_num++)
            {

                Console.Write(availableLetters[letter_num] + "  ");
            }
            Console.SetCursorPosition(53, 15);
            for (int letter_num = 20; letter_num < 26; letter_num++)
            {

                Console.Write(availableLetters[letter_num] + "  ");
            }

            for (int i = 0; i < 13; i++)
            {
                Console.Write(Ybox(51, i +5));
                Console.Write(Ybox(82, i + 5));
            }

            for (int alphabet = 0; alphabet < availableLetters.Length; alphabet++)
            {
                if (availableLetters[alphabet] == nextGuess)
                {
                    availableLetters[alphabet] = ' '; 
                }
            }
        }

        public void Run()
        {
            _renderer.Render(5, 5, 6);

            correctWord = GetWord().ToCharArray();

            UpdateLetterList();
            

            userGuesses.Clear();
            for (int i = 0; i < correctWord.Length; i++)
            {
                userGuesses.Add(DispBlank());
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            UpdateWordStatus();

            Console.ForegroundColor = ConsoleColor.Green;

            while (true)
            {
                UpdateLetterList();
                GetGuess();
                UpdateLetterList();

                if (CheckGuess())
                {
                    UpdateWordStatus();
                    Console.SetCursorPosition(0, 17);
                    Console.WriteLine("Nice! \n");
                }
                else
                {
                    numLives--;
                    Console.SetCursorPosition(0, 18);
                    Console.WriteLine($"Oopsie! {numLives} guesses remaining, try again...");
                    _renderer.Render(5, 5, numLives);
                    
                    if (numLives == 0)
                    {
                        _renderer.Render(5, 5, numLives);
                        Console.SetCursorPosition(5, 19);
                        Console.WriteLine($"You Died! \nThe Word is {new string(correctWord)}");
                        break;
                    }
                }
                IsWin();
                if (winCondition)
                {
                    Console.SetCursorPosition(9, 19);
                    Console.WriteLine("You WIN!");
                    break;
                }
            }
        }
    }
}