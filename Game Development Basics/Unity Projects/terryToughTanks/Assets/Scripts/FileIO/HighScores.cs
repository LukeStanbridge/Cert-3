using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class HighScores : MonoBehaviour
{
    public int[] scores = new int[10];

    string currentDirectory;

    public string scoreFileName = "highscores.txt";

    // Start is called before the first frame update
    void Start()
    {
        // Know where we're reading from and writing to. Print the current directory to console
        currentDirectory = Application.dataPath;
        Debug.Log("Our current directory is: " + currentDirectory);
        // Load the scores by default.
        LoadScoresFromFile();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            LoadScoresFromFile();
            Debug.Log(scoreFileName);
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            SaveScoresToFile();
        }
    }

    public void LoadScoresFromFile()
    {
        // Check file exists, if it doesn't then log a message and abort
        bool fileExists = File.Exists(currentDirectory + "\\" + scoreFileName);
        if (fileExists == true)
        {
            Debug.Log("Found high score file " + scoreFileName);
        }
        else
        {
            Debug.Log("The file " + scoreFileName + " does not exist. No scores will be loaded.", this);
            return;
        }
        // Make a new array of default values, ensures no old values stick around if we have loaded a score file before
        scores = new int[scores.Length];
        // Read the file using streamreader which we give our full file path to
        StreamReader fileReader;
        try
        {
            fileReader = new StreamReader(currentDirectory + "\\" + scoreFileName);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return;
        }
        // A counter so we don't go past the end of our scores
        int scoreCount = 0;
        // A while loop to run as long as there is data to be read AND we haven't reached the end of our scores array
        while (fileReader.Peek() != 0 && scoreCount < scores.Length)
        {
            // Read that line into a variable
            string fileLine = fileReader.ReadLine();

            // Try parse that variable into an int. First, make a variable to put it in
            int readScore = -1;
            bool didParse = int.TryParse(fileLine, out readScore);
            if (didParse)
            {
                // If we successfully read a number, put it in the array.
                scores[scoreCount] = 0;
            }
            else
            {
                // If number can't be parsed(probably junk in file). Print an error and use a default value.
                Debug.Log("Invalid line in scores file at " + scoreCount + ", using default value.", this);
                scores[scoreCount] = 0;
            }
            // Increment counter
            scoreCount++;
        }
        // CLose the stream
        fileReader.Close();
        Debug.Log("High scores read from " + scoreFileName);
    }
    public void SaveScoresToFile()
    {
        // Create a StreamWriter for our file path.
        StreamWriter fileWriter = new StreamWriter(currentDirectory + "\\" + scoreFileName);
        // Write the lines to the file
        for (int i = 0; i < scores.Length; i++)
        {
            fileWriter.WriteLine(scores[i]);
        }
        // Close the stream
        fileWriter.Close();
        // Write a log message
        Debug.Log("High scores written to " + scoreFileName);
    }

    public void AddScore(int newScore)
    {
        // Find what index it belongs at(this will be the first index with a score lower than the new score)
        int desiredIndex = -1;
        for (int i = 0; i < scores.Length; i++)
        {
            // Instead of checking the value of desiredIndex use a 'break' to stop the loop.
            if (scores[i] > newScore || scores[i] == 0)
            {
            desiredIndex = i;
            break;
            }
        }
        // If no desired index found then the score isn't high enough to get on the table, just abort
        if (desiredIndex < 0)
        {
            Debug.Log("Score of " + newScore + " not high enough for high scores list.", this);
            return;
        }
        // Move all the scores after that index back by one position(loop fromn the back of the array to desired index.)
        for (int i = scores.Length - 1; i > desiredIndex; i--)
        {
            scores[i] = scores[i - 1];
        }
        // Insert new score in its place
        scores[desiredIndex] = newScore;
        Debug.Log("Score of " + newScore + " enetered into high scores at position " + desiredIndex, this);
    }
}
