using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        private char[] letters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();

        private Random rdm_num = new Random();

        private char[] correct_word;

        bool win_condition;

        private char nextGuess;

        private List<char> user_guesses = new List<char>();

        private int num_lives = 6;

        private string get_word()
        {
            return word_list[rdm_num.Next(word_list.Length)];
        }

        private void get_guess()
        {
            Console.SetCursorPosition(0, 15);
            Console.Write("What is your next guess: ");
            nextGuess = Convert.ToChar(Console.ReadLine().ToLower());
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

        private string x_box(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            return "----------------------------------";
        }

        private string y_box(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            return "|";
        }

        private void update_letter_list() 
        {
            Console.Write(x_box(50,5));
            Console.Write(x_box(50, 17));

            Console.SetCursorPosition(60,7);
            Console.Write("Unused Letters:");

            Console.SetCursorPosition(53,9);
            for (int letter_num = 0; letter_num < 10; letter_num++)
            {
                
                Console.Write(letters[letter_num] +"  ");
            }
            Console.SetCursorPosition(53, 12);
            for (int letter_num = 10; letter_num < 20; letter_num++)
            {

                Console.Write(letters[letter_num] + "  ");
            }
            Console.SetCursorPosition(53, 15);
            for (int letter_num = 20; letter_num < 26; letter_num++)
            {

                Console.Write(letters[letter_num] + "  ");
            }

            for (int i = 0; i < 13; i++)
            {
                Console.Write(y_box(51, i +5));
                Console.Write(y_box(82, i + 5));
            }

            for (int alphabet = 0; alphabet < letters.Length; alphabet++)
            {
                if (letters[alphabet] == nextGuess)
                {
                    letters[alphabet] = ' '; 
                }
            }
        }

        public void Run()
        {
            _renderer.Render(5, 5, 6);

            correct_word = get_word().ToCharArray();

            update_letter_list();
            

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
                update_letter_list();
                get_guess();
                update_letter_list();

                if (check_guess())
                {
                    update_word_status();
                    Console.SetCursorPosition(0, 17);
                    Console.WriteLine("Nice! \n");
                }
                else
                {
                    num_lives--;
                    Console.SetCursorPosition(0, 18);
                    Console.WriteLine($"Oopsie! {num_lives} guesses remaining, try again...");
                    _renderer.Render(5, 5, num_lives);
                    
                    if (num_lives == 0)
                    {
                        _renderer.Render(5, 5, num_lives);
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

/*
 *TO DO:
 *fix functionality bug: Allow for input of words with repeated letters
 *
 *DONE:
 *display letter list of previously inputted letters --> remove used letters from list
*/ 