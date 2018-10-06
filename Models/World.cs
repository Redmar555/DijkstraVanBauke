using System;
using System.Collections.Generic;
using System.Linq;
using Controllers;

namespace Models
{
    public class World : IObservable<Command>, IUpdatable
    {
        public List<Abstract_Model> worldObjects = new List<Abstract_Model>();
        private List<IObserver<Command>> observers = new List<IObserver<Command>>();

        //Robot r;
        List<Robot> robots;
        //Trein t;
        Dijkstra d;
        public List<Node> NodeList = new List<Node>();
        public List<Storage> StorageSpots = new List<Storage>();
      
        public void addNode(char node, double x, double y, double z)
        {
            Node n = new Node(node, x, y, z);
            NodeList.Add(n);
        }
        public World()
        {
           
            // Create the graph, and create the nodes the robot can move to
            d = new Dijkstra();

            addNode('A', 0, 0, 0);
            addNode('B', 15, 0, 0);
            addNode('C', 30, 0, 0);
            addNode('D', 0, 0, 30);
            addNode('E', 30, 0, 30);

            Storage storage1 = new Storage(NodeList[0], 5, 0, 5, this);
            StorageSpots.Add(storage1);

            // g.shortest_path('A', 'H').ForEach(x => Console.WriteLine(x));
            SpawnTrein(-60, 0, -5);
            
            // List<char> paths = d.shortest_path('A','F');
            
            this.robots = new List<Robot>();
            for(int i = 0; i < 2; i++)
            {
                Robot r = CreateRobot((i * 2) + 15, 0, 0);
                this.robots.Add(r);
            }
            

            //CommandPickup();
           // MoveModel(r, 50, 0, 0);
        }
        //

        public void CommandPickup(Rek k, bool atTrain, Robot _r)
        {
            // Tell a robot to come pick up an item
            _r.idle = false;
            _r.rekToCarry = k;
            if (atTrain)
            {
                //Als B magazijn is en A trein
                // Move k (rekToCarry van de robot) van trein(A) naar  magazijn(B)
                _r.SetRoute(GenerateRoute('A', 'B'), 'B');
            } else
            {
                //Als B magazijn is en A trein
                // Move k (rekToCarry van de robot) van magazijn(B) naar trein(A)
                _r.SetRoute(GenerateRoute('B', 'A'), 'A');
            }
            
            //r.PickupRek();
        }
        /// <summary>
        /// Returns a route to a positon , and back again
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns> A list to and from the target</returns>
        public List<char> GenerateRoute(char start , char end)
        {
            List<char> Route = d.shortest_path(start,end);
            List<char> Terugweg = d.shortest_path(end,start);
            Route.Reverse();
            Terugweg.Reverse();
            Route.AddRange(Terugweg);
            return Route;
        }

        private Trein SpawnTrein(double x, double y, double z)
        {
            Trein t = new Trein(x, y, z, 0, 0, 0, this);
            t.Rotate(0, 89.55, 0);
            t.speed = 0.6;
            worldObjects.Add(t);
            //CreateRek(15,0,-30);
           
            return t;

        }

        public void TrainArrived(Trein _t, Rek cargo)
        {
            //Word aangeroepen wanneer een trein (_t) bij het loading dock is
            cargo.readyforpickup = true;
            
            //Loop door robots en laat een idle robot de cargo ophalen
            foreach(Robot r in this.robots)
            {
                if (r.idle)
                {
                    CommandPickup(cargo, true, r);
                    r.idle = false;
                    break;
                }
            }

            //Loop door robots een laat een idle robot een rek uit de storage naar de trein brengen
            foreach (Robot r in this.robots)
            {
                foreach (Storage s in this.StorageSpots)
                {
                    for (int i = 0; i < s.Stored.Count; i++)
                    {
                        Rek rek = s.Stored[i];
                        if (r.idle)
                        {
                            s.Stored.Remove(rek);
                            rek.readyforpickup = true;
                            r.trainToLoad = _t;
                            CommandPickup(rek, false, r);
                            r.idle = false;
                        }
                    }
                }
            }

        }

        public void TrainDeparted(Trein _t)
        {
            // Delete oude trein
            this.worldObjects.Remove(_t.CarriedRek);
            this.worldObjects.Remove(_t);

            //Maak nieuwe trein
            this.SpawnTrein(-60, 0, -5);
        }
            
        public Rek CreateRek(double x, double y, double z)
        {
            
            Rek rek = new Rek (x, y, z, 0, 0, 0);
            worldObjects.Add(rek);
            return rek;
        }
        private Robot CreateRobot(double x, double y, double z)
        {
            Robot constructorrobot = new Robot(x, y, z, 0, 0, 0,this);

            constructorrobot.idle = true;
            constructorrobot.speed = 0.6;


            worldObjects.Add(constructorrobot);
            return constructorrobot;
        }
        //public void MoveModel(Abstract_Model model,double x, double y , double z)
        //{
        //    //Check if you need to move on an axis
        //    // Check if you need to move less than a 'tick'
        //    // if true, move the  last remaining bit
        //    // if false, move the tick valye
        //    // Repeat until the move is done

        //    double xdif = x - model.x;
        //    double ydif = y - model.y;
        //    double zdif = z - model.z;
        //    bool destinationreached = false;
        //    // 3 times, for each axis
        //    while (!destinationreached)
        //    {


        //        if (model.needsUpdate)
        //        {



        //            // If not 0, i need to move on the X axis
        //            if (xdif != 0)
        //            {
        //                // If less than 5 , 
        //                if (xdif < 5)
        //                {
        //                    model.Move(xdif, 0, 0);
        //                    destinationreached = true;

        //                }
        //                else
        //                {
        //                    model.Move(5, 0, 0);
        //                    xdif = xdif - 5;
        //                }
        //            }

        //            if (xdif == 0)
        //            {
        //                destinationreached = true;
        //            }


        //        }
        //    }
        //}

        public IDisposable Subscribe(IObserver<Command> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);

                SendCreationCommandsToObserver(observer);
            }
            return new Unsubscriber<Command>(observers, observer);
        }

        private void SendCommandToObservers(Command c)
        {
            for (int i = 0; i < this.observers.Count; i++)
            {
                this.observers[i].OnNext(c);
            }
        }

        private void SendCreationCommandsToObserver(IObserver<Command> obs)
        {
            foreach (Abstract_Model m3d in worldObjects)
            {
                obs.OnNext(new UpdateModel3DCommand(m3d));
            }
        }

        public bool Update(int tick)
        {

            

            for (int i = 0; i < worldObjects.Count; i++)
            {
                Abstract_Model u = worldObjects[i];

                if (u is IUpdatable)
                {
                    bool needsCommand = ((IUpdatable)u).Update(tick);
                    if (needsCommand)
                    {
                        SendCommandToObservers(new UpdateModel3DCommand(u));
                    }
                }
            }
            return true;
        }
    }

    internal class Unsubscriber<Command> : IDisposable
    {
        private List<IObserver<Command>> _observers;
        private IObserver<Command> _observer;

        internal Unsubscriber(List<IObserver<Command>> observers, IObserver<Command> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
}