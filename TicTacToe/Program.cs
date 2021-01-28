using System;
using System.Collections.Generic;
using System.Reflection.Metadata;

namespace TicTacToe
{
    /// <summary>
    /// Auteur: Zachary Denis
    /// Description: Jeux tic tac toe
    /// Date: 2020-11-26
    /// </summary>
    class Program
    {
        #region Variables Globales
        private static string[,] _asTableau = new string[4, 4];
        private static Random _rnd = new Random();
        #endregion
        #region Méthodes
        /// <summary>
        /// Auteur: Zachary Denis
        /// Description: Méthodes qui gère le menu principal.
        /// Date: 2020-11-26
        /// </summary>
        private static char AfficheMenu()
        {
            // Variables locales.
            char cChoix = ' ';

            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" _____ _        _               _             ");
            Console.WriteLine("|_   _(_) ___  | |_ __ _  ___  | |_ ___   ___ ");
            Console.WriteLine("  | | | |/ __| | __/ _` |/ __| | __/ _ \\ / _ \\");
            Console.WriteLine("  | | | | (__  | || (_| | (__  | || (_) |  __/");
            Console.WriteLine("  |_| |_|\\___|  \\__\\__,_|\\___|  \\__\\___/ \\___|");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Est-ce que vous voulez: ");
            Console.WriteLine("1 - Jouer contre un humain");
            Console.WriteLine("2 - Faire un tournoi");
            Console.WriteLine("3 - Jouer contre un Intelligence artificielle (IA)");
            Console.WriteLine("Q - Quitter ");
            Console.Write("Votre choix: ");
            cChoix = Console.ReadKey().KeyChar;
            Console.WriteLine();
            // Swtich
            switch (Char.ToUpper(cChoix))
            {
                case '1':
                    JouerTicTacToe("Joueur #1", "Joueur #2");
                    break;
                case '2':
                    JouerTournoi();
                    break;
                case '3':
                    JouerTicTacToe("Joueur #1", "Ordinateur");
                    break;
                case 'Q':
                    break;
                default:
                    Console.WriteLine("ERREUR: Choix invalide!");
                    break;
            }

            return cChoix;
        }
        /// <summary>
        /// Auteur: Zachary Denis
        /// Description: Méthodes qui permet de faire jouer le tic tac toe
        /// Date: 2020-12-11
        /// </summary>
        /// <param name="sJoueur1">Le premier joueur</param>
        /// <param name="sJoueur2">Le deuxième joueur</param>
        /// <returns>Le gagnant du tictactoe</returns>
        private static string JouerTicTacToe(string sJoueur1, string sJoueur2)
        {
            // Variables locales.
            string sRéponse = "";
            string sJoueur = sJoueur1;
            string sGagnant = "";
            int iChoix = 0;
            byte[] abyCoord = null;
            bool bValide = false;
            string sNul = "";
            
            RemplirTableau();
            Console.Clear();
            do
            {
                Console.WriteLine(sJoueur1 + ": X et " + sJoueur2 + ": O");
                Console.WriteLine();
                Console.WriteLine("Tour du joueur: " + sJoueur);
                Console.WriteLine();
                AfficherGrid();
                if (sJoueur.Equals("Ordinateur"))
                {
                    do
                    {
                        sRéponse = _rnd.Next(1, 17).ToString();
                        abyCoord = CalculerCaseCoord(byte.Parse(sRéponse));
                    } while (_asTableau[abyCoord[0], abyCoord[1]] == "X" && _asTableau[abyCoord[0], abyCoord[1]] == "O");
                }
                else
                {
                    Console.Write("Entrer le numéro de la case: ");
                    sRéponse = Console.ReadLine();
                }


                if (int.TryParse(sRéponse, out iChoix) && sNul.Length != 16)
                {
                    if (iChoix <= 16 && iChoix >= 1)
                    {
                        abyCoord = CalculerCaseCoord((byte) iChoix);
                        if (!_asTableau[abyCoord[0], abyCoord[1]].Equals("X") &&
                            !_asTableau[abyCoord[0], abyCoord[1]].Equals("O"))
                        {
                            if (sJoueur.Equals(sJoueur1))
                            {
                                _asTableau[abyCoord[0], abyCoord[1]] = "X";
                                sJoueur = sJoueur2;
                            }
                            else
                            {
                                _asTableau[abyCoord[0], abyCoord[1]] = "O";
                                sJoueur = sJoueur1;
                            }

                            sGagnant = CheckWin(sJoueur1, sJoueur2);
                            if (sGagnant == "")
                                Console.Clear();
                        }
                        else
                            Console.WriteLine("Cette case est déjà occupé.");
                    }else
                        Console.WriteLine("La valeur doit être plus grand que 0 et plus petit que 16");
                }
                else
                    Console.WriteLine("La valeur entrer n'est pas numérique.");

                if (sGagnant != "")
                    bValide = true;
                sNul = "";
                foreach (string item in _asTableau)
                {
                    if (item == "X" || item == "O")
                        sNul += "X";
                }
            } while (!bValide && sNul.Length != 16);
            Console.Clear();
            AfficherGrid();
            if (sNul.Length != 16 || sGagnant.Length >= 1)
                Console.WriteLine("Le gagnant de la partie est: " + sGagnant);
            else
                Console.WriteLine("La partie est nulle!");
            Console.Write("Appuyez sur une touche pour continuer!");
            Console.ReadKey();
            Console.Clear();
            return sGagnant;
        }
        /// <summary>
        /// Auteur: Zachary Denis
        /// Description: Convertir un numéro de case en coordonnées.
        /// Date: 2020-12-04
        /// </summary>
        /// <param name="byNoCase">Numéro de case entre 1 et N (N=Nombre total de cases)</param>
        /// <returns>Tableau avec les coordonnées. [0]=Ligne, [1]=Colonne</returns>
        static private byte[] CalculerCaseCoord(byte byNoCase)
        {
            // Variables locales.
            byte[] abyCoor = new byte[2]; // 0 = Ligne, 1 = Colonne;

            if (byNoCase >= 1 && byNoCase <= _asTableau.Length)
            {
                abyCoor[0] = (byte) ((byNoCase - 1) / _asTableau.GetLength(0)); // Ligne
                abyCoor[1] = (byte) ((byNoCase - 1) % _asTableau.GetLength(1)); // Colonne
            }
            else
                return null;

            return abyCoor;
        }
        /// <summary>
        /// Auteur: Zachary Denis
        /// Description: Méthodes qui gère le partis de tournois
        /// Date: 2020-12-11
        /// </summary>
        private static void JouerTournoi()
        {
            // Variables locales.
            string sChoix = "";
            string sGagnant = "";
            List<string> participants = new List<string>();
            
            // On demande le nombre de participant.
            do
            {
                Console.Write("Veuillez entrer le nombre de participant. (4 = 4 participants et 8 = 8 partipants): ");
                sChoix = Console.ReadLine();
                Console.WriteLine();
                if(!sChoix.Equals("4") && !sChoix.Equals("8"))
                    Console.WriteLine("Mauvais choix! Veuillez réessayer.");
            } while (!sChoix.Equals("4") && !sChoix.Equals("8"));
            
            for (int iCpt = 0; iCpt < int.Parse(sChoix); iCpt++)
            {
                Console.Write("Entrer le nom du joueur #" + (iCpt + 1) + ": ");
                participants.Add(Console.ReadLine());
            }

            if (sChoix == "4")
            {
                for (int iCpt = 0; iCpt < 4; iCpt += 2)
                {
                    sGagnant = "";
                    AfficherGridTournoi(participants);
                    do
                    {
                        sGagnant = JouerTicTacToe(participants[iCpt], participants[iCpt + 1]);
                    } while (sGagnant.Length == 0);
                    participants.Add(sGagnant);
                }
                AfficherGridTournoi(participants);
                sGagnant = "";
                do
                {
                    sGagnant = JouerTicTacToe(participants[4], participants[5]);
                } while (sGagnant.Length == 0);
                Console.WriteLine("Le grand gagnant du tournoi est: " + sGagnant);
                Console.Write("Appuyez sur une touche pour continuer!");
                Console.ReadKey();
            }else if (sChoix == "8")
            {
                for (int iCpt = 0; iCpt < 12; iCpt += 2)
                {
                    sGagnant = "";
                    AfficherGridTournoi(participants);
                    do
                    {
                        sGagnant = JouerTicTacToe(participants[iCpt], participants[iCpt + 1]);
                    } while (sGagnant.Length == 0);
                    participants.Add(sGagnant);
                }
                AfficherGridTournoi(participants);
                sGagnant = "";
                do
                {
                    sGagnant = JouerTicTacToe(participants[12], participants[13]);
                } while (sGagnant.Length == 0);
                Console.WriteLine("Le grand gagnant du tournoi est: " + sGagnant);
                Console.Write("Appuyez sur une touche pour continuer!");
                Console.ReadKey();
            }
        }
        /// <summary>
        /// Auteur: Zachary Denis
        /// Description: Méthodes qui affiche le jeu de tic tac toe
        /// Date: 2020-12-11
        /// </summary>
        private static void AfficherGrid()
        {
            // Variables locales.
            int iNoLigne = 0;
            int iNoColonne = 0;

            for (int iLigne = 0; iLigne < 4; iLigne++)
            {
                for (int iColonne = 0; iColonne < 4; iColonne++)
                {
                    for (int z = 0; z < 5; z++)
                    {
                        Console.Write(" ");
                    }

                    if (iColonne != 3)
                        Console.Write("|");
                }

                Console.WriteLine();
                for (int iColonne = 0; iColonne < 4; iColonne++)
                {
                    if (_asTableau[iNoLigne, iNoColonne].Length == 2)
                    {
                        for (int iCpt = 0; iCpt < 4; iCpt++)
                        {
                            if (iCpt != 2)
                                Console.Write(" ");
                            else
                            {
                                if(_asTableau[iNoLigne, iNoColonne] == "X")
                                {
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.Write(_asTableau[iNoLigne, iNoColonne]);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                }
                                else if (_asTableau[iNoLigne, iNoColonne] == "O")
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.Write(_asTableau[iNoLigne, iNoColonne]);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                }
                                else
                                    Console.Write(_asTableau[iNoLigne, iNoColonne]);
                            }
                        }
                    }
                    else
                    {
                        for (int iCpt = 0; iCpt < 5; iCpt++)
                        {
                            if (iCpt != 2)
                                Console.Write(" ");
                            else
                            {
                                if (_asTableau[iNoLigne, iNoColonne] == "X")
                                {
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.Write(_asTableau[iNoLigne, iNoColonne]);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                }
                                else if (_asTableau[iNoLigne, iNoColonne] == "O")
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.Write(_asTableau[iNoLigne, iNoColonne]);
                                    Console.ForegroundColor = ConsoleColor.Green;
                                }
                                else
                                    Console.Write(_asTableau[iNoLigne, iNoColonne]);
                            }
                        }
                    }

                    iNoColonne++;
                    if (iNoColonne == _asTableau.GetLength(1))
                    {
                        iNoColonne = 0;
                        iNoLigne++;
                    }

                    if (iColonne != 3)
                        Console.Write("|");
                }

                Console.WriteLine();
                if (iLigne != 3)
                {
                    for (int iColonne = 0; iColonne < 4; iColonne++)
                    {
                        for (int iCpt = 0; iCpt < 5; iCpt++)
                        {
                            Console.Write("_");
                        }

                        if (iColonne != 3)
                            Console.Write("|");
                    }
                }
                else
                {
                    for (int iColonne = 0; iColonne < 4; iColonne++)
                    {
                        for (int z = 0; z < 5; z++)
                        {
                            Console.Write(" ");
                        }

                        if (iColonne != 3)
                            Console.Write("|");
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();
            }

            Console.WriteLine();
        }
        /// <summary>
        /// Auteur: Zachary Denis
        /// Description: Méthodes qui affiche la grille du tournoi
        /// Date: 2020-12-11
        /// </summary>
        /// <param name="participants">Liste des participants pour le tournoi</param>
        private static void AfficherGridTournoi(List<string> participants)
        {
            // Variables locales.
            int iNombre = participants.Count;
            int iPadRight = 15;
            Console.Clear();
            Console.WriteLine("==== Tableau du tournoi ====");
            Console.WriteLine();
            if (iNombre <= 6)
            {
                for (int iCpt = 0; iCpt < 3; iCpt++)
                {
                    if (iCpt != 2)
                    {
                        Console.Write(participants[iCpt].PadRight(iPadRight, ' '));
                        Console.WriteLine("\\");
                        iPadRight++;
                    }
                }
                iPadRight++;
                Console.Write("\\".PadLeft(iPadRight, ' '));
                if(participants.Count >= 5)
                    Console.WriteLine(participants[4].PadLeft(participants[4].Length + 6, ' '));
                else
                    Console.WriteLine("À déterminer".PadLeft(18, ' '));
                Console.Write("/".PadLeft(iPadRight, ' '));
                if(participants.Count >= 6)
                    Console.WriteLine(participants[5].PadLeft(participants[5].Length + 6, ' '));
                else
                    Console.WriteLine("À déterminer".PadLeft(18, ' '));
                iPadRight--;
                iPadRight--;
                for (int iCpt = 2; iCpt < 4; iCpt++)
                {
                    Console.Write(participants[iCpt].PadRight(iPadRight, ' '));
                    Console.WriteLine("/");
                    iPadRight--;
                }
            }else if (iNombre >= 8)
            {
                for (int iCpt = 0; iCpt < 3; iCpt++)
                {
                    if (iCpt != 2)
                    {
                        Console.Write(participants[iCpt].PadRight(iPadRight, ' '));
                        Console.WriteLine("\\");
                        iPadRight++;
                    }
                }
                iPadRight++;
                Console.Write("\\".PadLeft(iPadRight, ' '));
                if(participants.Count >= 9)
                    Console.WriteLine(participants[8].PadLeft(participants[8].Length + 6, ' '));
                else
                    Console.WriteLine("À déterminer".PadLeft(18, ' '));
                Console.Write("/".PadLeft(iPadRight, ' '));
                if(participants.Count >= 10)
                    Console.WriteLine(participants[9].PadLeft(participants[9].Length + 6, ' '));
                else
                    Console.WriteLine("À déterminer".PadLeft(18, ' '));
                iPadRight--;
                iPadRight--;
                for (int iCpt = 2; iCpt < 4; iCpt++)
                {
                    Console.Write(participants[iCpt].PadRight(iPadRight, ' '));
                    Console.WriteLine("/");
                    iPadRight--;
                }
                
                Console.WriteLine("\\".PadLeft(40, ' '));
                Console.WriteLine("\\".PadLeft(41, ' '));
                Console.Write("\\".PadLeft(42, ' '));
                if(participants.Count >= 13)
                    Console.WriteLine(participants[12].PadLeft(participants[12].Length + 6, ' '));
                else
                    Console.WriteLine("À déterminer".PadLeft(18, ' '));
                Console.Write("/".PadLeft(42, ' '));
                if(participants.Count >= 14)
                    Console.WriteLine(participants[13].PadLeft(participants[13].Length + 6, ' '));
                else
                    Console.WriteLine("À déterminer".PadLeft(18, ' '));
                Console.WriteLine("/".PadLeft(41, ' '));
                Console.WriteLine("/".PadLeft(40, ' '));
                
                iPadRight = 15;
                for (int iCpt = 4; iCpt < 6; iCpt++)
                {
                    if (iCpt != 2)
                    {
                        Console.Write(participants[iCpt].PadRight(iPadRight, ' '));
                        Console.WriteLine("\\");
                        iPadRight++;
                    }
                }
                iPadRight++;
                Console.Write("\\".PadLeft(iPadRight, ' '));
                if(participants.Count >= 11)
                    Console.WriteLine(participants[10].PadLeft(participants[10].Length + 6, ' '));
                else
                    Console.WriteLine("À déterminer".PadLeft(18, ' '));
                Console.Write("/".PadLeft(iPadRight, ' '));
                if(participants.Count >= 12)
                    Console.WriteLine(participants[11].PadLeft(participants[11].Length + 6, ' '));
                else
                    Console.WriteLine("À déterminer".PadLeft(18, ' '));
                iPadRight--;
                iPadRight--;
                for (int iCpt = 6; iCpt < 8; iCpt++)
                {
                    Console.Write(participants[iCpt].PadRight(iPadRight, ' '));
                    Console.WriteLine("/");
                    iPadRight--;
                }
            }
            Console.Write("Appuyez sur une touche pour continuer!");
            Console.ReadKey();
        }
        /// <summary>
        /// Auteur: Zachary Denis
        /// Description: Méthodes qui regarde si un joueur a gagner.
        /// Date: 2020-12-11
        /// </summary>
        /// <param name="sJoueur1">Le premier joueur</param>
        /// <param name="sJoueur2">Le deuxième joueur</param>
        /// <returns>Le joueur gagnant</returns>
        private static string CheckWin(string sJoueur1, string sJoueur2)
        {
            // Variables locales.
            string sGagnant = "";
            string sJeu = "";
            string sPossibilitéX = "XXXX";
            string sPossibilitéO = "OOOO";

            for (int iLigne = 0; iLigne < _asTableau.GetLength(0); iLigne++)
            {
                for (int iColonne = 0; iColonne < _asTableau.GetLength(1); iColonne++)
                {
                    if (_asTableau[iLigne, iColonne].Equals("X"))
                    {
                        sJeu += "X";
                    }
                    else if (_asTableau[iLigne, iColonne].Equals("O"))
                    {
                        sJeu += "O";
                    }

                    if (sJeu == sPossibilitéX)
                    {
                        sGagnant = sJoueur1;
                        return sGagnant;
                    }

                    if (sJeu == sPossibilitéO)
                    {
                        sGagnant = sJoueur2;
                        return sGagnant;
                    }
                }

                sJeu = "";
            }

            for (int iLigne = 0; iLigne < _asTableau.GetLength(1); iLigne++)
            {
                for (int iColonne = 0; iColonne < _asTableau.GetLength(0); iColonne++)
                {
                    if (_asTableau[iColonne, iLigne].Equals("X"))
                    {
                        sJeu += "X";
                    }
                    else if (_asTableau[iColonne, iLigne].Equals("O"))
                    {
                        sJeu += "O";
                    }

                    if (sJeu == sPossibilitéX)
                    {
                        sGagnant = sJoueur1;
                        return sGagnant;
                    }

                    if (sJeu == sPossibilitéO)
                    {
                        sGagnant = sJoueur2;
                        return sGagnant;
                    }
                }

                sJeu = "";
            }

            if (_asTableau[0, 0].Equals("X") || _asTableau[0, 0].Equals("O"))
                sJeu += _asTableau[0, 0];
            if (_asTableau[1, 1].Equals("X") || _asTableau[1, 1].Equals("O"))
                sJeu += _asTableau[1, 1];
            if (_asTableau[2, 2].Equals("X") || _asTableau[2, 2].Equals("O"))
                sJeu += _asTableau[2, 2];
            if (_asTableau[3, 3].Equals("X") || _asTableau[3, 3].Equals("O"))
                sJeu += _asTableau[3, 3];
            if (sJeu == sPossibilitéX)
            {
                sGagnant = sJoueur1;
                return sGagnant;
            }

            if (sJeu == sPossibilitéO)
            {
                sGagnant = sJoueur2;
                return sGagnant;
            }
            sJeu = "";
            if (_asTableau[0, 3].Equals("X") || _asTableau[0, 3].Equals("O"))
                sJeu += _asTableau[0, 3];
            if (_asTableau[1, 2].Equals("X") || _asTableau[1, 2].Equals("O"))
                sJeu += _asTableau[1, 2];
            if (_asTableau[2, 1].Equals("X") || _asTableau[2, 1].Equals("O"))
                sJeu += _asTableau[2, 1];
            if (_asTableau[3, 0].Equals("X") || _asTableau[3, 0].Equals("O"))
                sJeu += _asTableau[3, 0];

            if (sJeu == sPossibilitéX)
            {
                sGagnant = sJoueur1;
                return sGagnant;
            }

            if (sJeu == sPossibilitéO)
            {
                sGagnant = sJoueur2;
                return sGagnant;
            }

            return sGagnant;
        }
        /// <summary>
        /// Auteur: Zachary Denis
        /// Description: Méthodes qui initialise les case du tableau de jeu.
        /// Date: 2020-12-11
        /// </summary>
        private static void RemplirTableau()
        {
            int iNombre = 1;
            for (int iNbLigne = 0; iNbLigne < _asTableau.GetLength(0); iNbLigne++)
            for (int iNbColonne = 0; iNbColonne < _asTableau.GetLength(1); iNbColonne++)
            {
                _asTableau[iNbLigne, iNbColonne] = iNombre.ToString();
                iNombre++;
            }
        }
        #endregion
        static void Main(string[] args)
        {
            char cChoix = ' ';
            do
            {
                cChoix = AfficheMenu();
                if (char.ToUpper(cChoix) == 'Q')
                    Console.WriteLine("Au revoir!");
            } while (char.ToUpper(cChoix) != 'Q');
        }
    }
}