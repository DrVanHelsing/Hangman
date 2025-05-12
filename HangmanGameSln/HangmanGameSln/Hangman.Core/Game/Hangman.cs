using System;
using System.Collections.Generic;
using System.Data;
using HangmanRenderer.Renderer;

namespace Hangman.Core.Game
{
    public class HangmanGame
    {
        private GallowsRenderer _renderer;

        public HangmanGame()
        {
            _renderer = new GallowsRenderer();
        }

        private string[] word_list = { "help", "community", "computer" };

        private char[] correct_word;

        private string my_word = String.Empty;

        private char nextGuess;

        private List<char> user_guesses = new List<char>();

        private int wrong_guess = 5;

        private string get_word()
        {
            Random rdm_num = new Random();
            my_word = word_list[rdm_num.Next(word_list.Length)];
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

        public void Run()
        {
            _renderer.Render(5, 5, 6);

            correct_word = get_word().ToCharArray();

            for (int i = 0; i < correct_word.Length; i++)
            {
                user_guesses.Add(disp_blank());
            }

            Console.ForegroundColor = ConsoleColor.Blue;

            update_word_status();

            Console.ForegroundColor = ConsoleColor.Green;
            get_guess();

            while (true)
            {
                if (check_guess() == true)
                {
                    update_word_status();
                    Console.SetCursorPosition(0, 17);
                    Console.WriteLine("Nice!");
                    get_guess();
                }
                else if (check_guess() == false)
                {
                    Console.SetCursorPosition(0, 17);
                    Console.WriteLine($"Oopsie! {wrong_guess} guesses remaining, try again...");
                    
                    _renderer.Render(5, 5, wrong_guess);
                    wrong_guess--;
                    get_guess();

                    if (wrong_guess == 0)
                    {
                        _renderer.Render(5, 5, wrong_guess);
                        Console.SetCursorPosition(5, 19);
                        Console.WriteLine($"You Died! \nThe Word is {my_word}");
                        break;
                    }
                }
            }


        }

    }
}
