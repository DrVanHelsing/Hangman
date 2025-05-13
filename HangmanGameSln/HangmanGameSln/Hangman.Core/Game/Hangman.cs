using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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

        private string[] word_list = { "help", "build", "computer" };

        private Random rdm_num = new Random();

        private char[] correct_word;

        bool win_condition;

        private char nextGuess;

        private List<char> user_guesses = new List<char>();

        private int wrong_guess = 6;

        private string get_word()
        {
            return word_list[rdm_num.Next(word_list.Length)];

        }

        private void get_guess()
        {
            Console.SetCursorPosition(0, 15);
            Console.Write("What is your next guess: ");
            nextGuess = Convert.ToChar(Console.ReadLine());
        }

        private bool check_guess()
        {
            for (int i = 0; i < correct_word.Length; i++)
            {
                if (nextGuess == correct_word[i])
                {
                    user_guesses[i] = nextGuess;

                    return true;
                }
            }
            return false;
        }

        private char disp_blank()
        {
            return '_';
        }

        private void update_word_status()
        {
            for (int j = 0; j < correct_word.Length; j++)
            {
                Console.SetCursorPosition(0, 13);
                Console.Write("Your current guess: ");
                Console.Write(string.Join(" ", user_guesses));
            }
        }

        private void is_win()
        {
            win_condition = true;
            for (int i = 0; i < correct_word.Length; i++)
            {
                if ((user_guesses[i].Equals(correct_word[i])) == false)
                {
                    win_condition = false;
                    break;
                }
            }
        }

        public void Run()
        {
            _renderer.Render(5, 5, 6);

            correct_word = get_word().ToCharArray();

            user_guesses.Clear();
            for (int i = 0; i < correct_word.Length; i++)
            {
                user_guesses.Add(disp_blank());
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            update_word_status();

            Console.ForegroundColor = ConsoleColor.Green;

            while (true)
            {
                get_guess();

                if (check_guess())
                {
                    update_word_status();
                    Console.SetCursorPosition(0, 17);
                    Console.WriteLine("Nice!");
                }
                else
                {
                    wrong_guess--;
                    Console.SetCursorPosition(0, 18);
                    Console.WriteLine($"Oopsie! {wrong_guess} guesses remaining, try again...");
                    _renderer.Render(5, 5, wrong_guess);
                    
                    if (wrong_guess == 0)
                    {
                        _renderer.Render(5, 5, wrong_guess);
                        Console.SetCursorPosition(5, 19);
                        Console.WriteLine($"You Died! \nThe Word is {new string(correct_word)}");
                        break;
                    }
                }

                is_win();
                if (win_condition)
                {
                    Console.SetCursorPosition(9, 19);
                    Console.WriteLine("You WIN!");
                    break;
                }
            }
        }

    }
}
