using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShuffleList
{
    // Суть этого класса заключается в том, чтобы с OriginalList элементы сделать в рассыпную. Чтобы каждый элемент рандомно находился в списке
    public static List<E> ShuffleListItems<E>(List<E> inputList) // inputList - это массив вариантов ответа
    {
        List<E> originalList = new List<E>(); // Создаём оригинальный список
        originalList.AddRange(inputList); // Добавляем в список массив вариантов ответа, который был в inputList
        List<E> randomList = new List<E>(); // Создаём рандомный список

        System.Random r = new System.Random(); //  Добавляем директиву в переменную
        int randomIndex = 0; // Рандомный индекс
        while (originalList.Count > 0) // если количеcтво оригинального массива больше нуля
        {
            randomIndex = r.Next(0, originalList.Count); // В индекс добавляем через Random.Next любое число до длины массива originalList
            randomList.Add(originalList[randomIndex]); // В randomList добавляем список массива под индексом, который мы нашли
            originalList.RemoveAt(randomIndex); //Удалить элемент с оригинального списка
        }

        return randomList; // Возвращаем новый случайный список
    }
}

