using System;

namespace CSDP399
{
    class Program
    {
        static byte func(byte[] sol, byte sol_idx, byte[] num, byte[,] flags)
        {
            for (byte b = 0; b < 4; ++b)
            {
                if (flags[sol_idx, b] == 0) // num is available
                {
                    sol[sol_idx] = num[b];
                    double val = 0;
                    byte pow = sol_idx;
                    for (byte c = 0; c < sol_idx + 1; ++c) // calc val
                    {
                        val += sol[c] * Math.Pow(10, pow);
                        --pow;
                    }
                    if (val % (sol_idx + 1) == 0) // the num worked
                    {
                        if (sol_idx % 2 == 0)  // num used up.
                        { 
                            for (byte c = 0; c < 9; c += 2)
                                if (flags[c, b] != 2)
                                    flags[c, b] = 1;
                        }
                        else // same, just being efficient.
                        {
                            for (byte c = 1; c < 8; c += 2)
                                if (flags[c, b] != 2)
                                    flags[c, b] = 1;
                        } 
                        return 9; // we can honestly use any num here > 8. we use 9 to denote that the operation was successful.
                    }
                    else flags[sol_idx, b] = 2; // set flag to fail since it didnt work.
                }                
            }
            return sol_idx; // nothing in the num array worked :-(
        }
        static void Main(string[] args)
        {
            byte[] sol = new byte[10]; // solution array.
            byte[] o_num = new byte[4]; // number choices.
            byte[] e_num = new byte[4];
            byte[,] flags = new byte[9,4]; // flags: 0 = unused, 1 = used, 2 = failure.
            sol[4] = 5; sol[9] = 0; // we know where 5 and 0 belong.
            o_num[0] = 1; o_num[1] = 3; o_num[2] = 7; o_num[3] = 9; // possible nums for the odd spots.
            e_num[0] = 2; e_num[1] = 4; e_num[2] = 6; e_num[3] = 8; // possible nums for the even spots.
            for (byte b = 0; b < 9; ++b) // init flags
            {
                if (b == 4);
                else {flags[b, 0] = 0; flags[b, 1] = 0; flags[b, 2] = 0; flags[b, 3] = 0;}
            }
            byte sol_idx;
            for (byte b = 0; b < 9; ++b) // we stop at 9 because any number that ends in 0 is divisible by 10.
            {
                if (b == 4); // skip over 5.
                else if (b % 2 == 0) // if the position is even (has an odd num, index starts at 0) 
                {
                    sol_idx = func(sol, b, o_num, flags);
                    if (sol_idx != 9)
                    {
                        for (byte c = 0; c < 4; ++c)
                        {
                            if (e_num[c] == sol[sol_idx - 1])
                            {
                                for (byte d = 1; d < 8; d += 2)  // unload "used" flags for num
                                    if (flags[d, c] != 2)
                                        flags[d, c] = 0;
                                flags[sol_idx - 1, c] = 2; // set the fail flag on the num at the prev sol_idx
                                break;
                            }
                        }
                        for (byte c = 0; c < 4; ++c)  // clear current index fail flags so nums will be free to use
                            if (flags[sol_idx, c] == 2)
                                flags[sol_idx, c] = 0;
                        b = (byte)(sol_idx - 2); // we go back two spaces because for loop will add 1 after cycle.
                    }
                }
                else
                {
                    sol_idx = func(sol, b, e_num, flags);
                    if (sol_idx != 9)
                    {
                        if (sol_idx != 5)
                        {
                            for (byte c = 0; c < 4; ++c)
                            {
                                if (o_num[c] == sol[sol_idx - 1])
                                {
                                    for (byte d = 0; d < 9; d += 2)
                                        if (flags[d, c] != 2)
                                            flags[d, c] = 0;
                                    flags[sol_idx - 1, c] = 2;
                                    break;
                                }
                            }							
                        }
                        else
                        {
                            for (byte c = 0; c < 4; ++c)
                            {
                                if (e_num[c] == sol[sol_idx - 2])
                                {
                                    for (byte d = 1; d < 8; d += 2)
                                        if (flags[d, c] != 2)
                                            flags[d, c] = 0;
                                    flags[sol_idx - 2, c] = 2;
                                    break;
                                }
                            }
                        }
                        for (byte c = 0; c < 4; ++c)
                            if (flags[sol_idx, c] == 2)
                                flags[sol_idx, c] = 0;
                        if (sol_idx != 5) b = (byte)(sol_idx - 2); // dont want to run into 5.
                        else b = (byte)(sol_idx - 3);
                    }
                }
            }
            Console.Write("Solution: ");
            for (byte b = 0; b < 10; ++b) Console.Write(sol[b]);
            Console.ReadKey();
        }
    }
}