using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;

namespace LeitourApi.Data;

public class Cryptography
{
    const int ERROR = -1;
    const int SHIFTVALUE = 10;
    const int PRIME = 241;

    /*
        Program flow:
            Receber mensagem
            *Converter para que a mensagem só tenha a-z,A-Z e 0-9
            Transpor os caracteres da mensagem
            *Gerar um primo de acordo com o tamanho da mensagem
            Multiplicar com o primo padrão para gerar a "seed"
            Usar essa Seed para alterar os Valores dos caracteres
            -- Colocar a seed a mensagem --

            Como descriptografar
                Retirar a seed da mensage
                Dividir pelo primo padrão
                Rodar o algorimo que usa a seed ao contrário
                Destranspor os caracteres da mensagem

    */
    public static string Criptografar(string message)
    {
        FormatMessage(message);
        return "";
    }

    public static string FormatMessage(string message)
    {
        Regex rgx = new("[^a-zA-Z0-9]");
        return rgx.Replace(message, "");
    }

    private static int ConvertCharacter(char character)
    {
        int characterValue = Convert.ToInt32(character);
        if (characterValue >= 48 && characterValue <= 57)
            return characterValue - 48;
        else if (characterValue >= 65 && characterValue <= 90)
            return characterValue - 55;
        else if (characterValue >= 97 && characterValue <= 122)
            return characterValue - 61;
        else
            return ERROR;
    }
    private static string ConvertInt(int character) => char.ConvertFromUtf32(character);


    private static int ShiftCharacter(int characterValue, int position)
    {
        characterValue += (position % 2 == 0) ? SHIFTVALUE : - SHIFTVALUE;
        if (characterValue > 122)
            characterValue -= 122;
        else if (characterValue < 0)
            characterValue += 122;
        return VerifyShift(characterValue);
    }

    private static int ShiftPosition(int position, int size)
    {
        int newPosition = position * 2;
        return (newPosition > size) ? newPosition - (size + 1) : newPosition;
    }

    private static int VerifyShift(int characterValue)
    {
        if (characterValue > 122)
            return characterValue -= 122;
        if (characterValue < 0)
            return characterValue += 122;
        return characterValue;
    }

    private static int GeneratePrime(int size)
    {
        int number = (size % 2 == 0) ? size + 1 : size + 2;
        for (int i = 3; i < number; i += 2)
        {
            if (number % i == 0)
            {
                number += 2;
                i = 1;
            }
        }
        return number;
    }
}
