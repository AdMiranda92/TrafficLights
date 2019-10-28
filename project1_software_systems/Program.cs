using System;
using System.Diagnostics;

//
// COP4365 – Fall Semester, 2019
//
// Homework #2: A Smarter Stoplight Problem
//
// Adrian Miranda
//
class TrafficLight { 

    private string color = "Red";
    private string name;

    //
    // Method Name: TrafficLight
    // Description: Constructor for traffic light object
    public TrafficLight(string initialColor, string name)
    {
        this.name = name + " Light";
        color = initialColor;
    }

    //
    // Method Name: setColor
    // Description: method to set the color of the light
    public void setColor(string i)
    {
        color = i;
    }

    //
    // Method Name: getColor
    // Description: method to get the current color of the light
    public string getColor()
    {
        return color;
    }

    //
    // Method Name: getName
    // Description: method to get the name of the light
    public string getName()
    {
        return name;
    }
}

namespace project1_software_systems
{
    class Program
    {
        static void Main(string[] args)
        {
            // initial colors and lights
            TrafficLight North = new TrafficLight("Green", "North");
            TrafficLight South = new TrafficLight("Red", "South");
            TrafficLight East = new TrafficLight("Red", "East");
            TrafficLight West = new TrafficLight("Red", "West");

            // array to hold the lights in sequence
            TrafficLight[] intersection = { North, South, East, West };
            // print out table headings
            Console.WriteLine("Current Time" + "\t   " + North.getName() + "\t   " + South.getName() + "\t   " +
                East.getName() + "\t   " + West.getName());


            // creating 2 stopwatches, one for overall time
            // and one for incremental time
            Stopwatch stopWatchOverall = new Stopwatch();
            Stopwatch stopWatchInc = new Stopwatch();

            // creating a var to track current light being manipulated
            int i = 0;
            TrafficLight current = intersection[i];

            // value to check if any changes were made
            // to avoid printing unnecessary iterations
            bool changeFlag = false;

            
            stopWatchOverall.Start();
            stopWatchInc.Start();
            TimeSpan tsOverall = stopWatchOverall.Elapsed;
            TimeSpan tsInc = stopWatchInc.Elapsed;

            // var to check for emergency vehicle
            bool emergency = false;
            while (tsOverall.Minutes < 1)
            {
                if(tsOverall.Seconds == 35)
                {
                    emergency = true;
                    System.Threading.Thread.Sleep(1000);
                    current.setColor("Red");
                    intersection[(i + 1) % 4].setColor("Red");
                    i = 2;
                    current = intersection[i];
                    current.setColor("Green");
                    Console.WriteLine("An emergency vehicle has been detected coming from the East");
                    Console.WriteLine("\t" + tsOverall.Seconds + "\t\t" + intersection[0].getColor() + "\t\t" + intersection[1].getColor() +
                        "\t\t" + intersection[2].getColor() + "\t\t" + intersection[3].getColor());
                    stopWatchInc.Stop();
                    stopWatchInc.Reset();
                    stopWatchInc.Start();
                }

                if (emergency)
                {
                    while(tsInc.Seconds < 9)
                    {
                        System.Threading.Thread.Sleep(1000);
                        tsOverall = stopWatchOverall.Elapsed;
                        tsInc = stopWatchInc.Elapsed;
                    }
                    Console.WriteLine("The emergency vehicle has left the area");
                    Console.WriteLine("\t" + tsOverall.Seconds + "\t\t" + intersection[0].getColor() + "\t\t" + intersection[1].getColor() +
                        "\t\t" + intersection[2].getColor() + "\t\t" + intersection[3].getColor());
                    stopWatchInc.Stop();
                    stopWatchInc.Reset();
                    stopWatchInc.Start();
                    emergency = false;

                }

                if (tsOverall.Seconds == 0)
                {
                    Console.WriteLine("\t" + tsOverall.Seconds + "\t\t" + intersection[0].getColor() + "\t\t" + intersection[1].getColor() +
                        "\t\t" + intersection[2].getColor() + "\t\t" + intersection[3].getColor());
                    System.Threading.Thread.Sleep(1000);
                    tsOverall = stopWatchOverall.Elapsed;
                    continue;
                }

                tsInc = stopWatchInc.Elapsed;
                // if the current light is north or east
                if (i % 2 == 0)
                {
                    if(tsInc.Seconds == 6)
                    {
                        intersection[(i + 1) % 4].setColor("Green");
                        changeFlag = true;
                    }

                    if(tsInc.Seconds == 9)
                    {
                        current.setColor("Yellow");
                        changeFlag = true;
                    }

                    if(tsInc.Seconds == 12)
                    {
                        current.setColor("Red");
                        changeFlag = true;
                        i++;
                        current = intersection[i%4];
                        stopWatchInc.Stop();
                        stopWatchInc.Reset();
                        stopWatchInc.Start();
                    }
                }
                else // current light is south or west
                {
                    // if 3 seconds have passed and the current light is green
                    // turn the light yellow
                    if (tsInc.Seconds == 3)
                    {
                        current.setColor("Yellow");
                        changeFlag = true;
                          
                    }

                    // if 6 seconds have passed and the current light is yellow
                    // turn the current light red and set the next light to green
                    if (tsInc.Seconds == 6)
                    {
                        current.setColor("Red");
                        changeFlag = true;
                        i++;
                        current = intersection[i % 4];
                        current.setColor("Green");
                        stopWatchInc.Stop();
                        stopWatchInc.Reset();
                        stopWatchInc.Start();
                    }
                }

                if (changeFlag)
                {
                    Console.WriteLine("\t" + tsOverall.Seconds + "\t\t" + intersection[0].getColor() + "\t\t" + intersection[1].getColor() +
                        "\t\t" + intersection[2].getColor() + "\t\t" + intersection[3].getColor());
                    changeFlag = false;
                    System.Threading.Thread.Sleep(1000);
                }

                tsOverall = stopWatchOverall.Elapsed;
            }
            stopWatchOverall.Stop();            
            
        }
    }
}
