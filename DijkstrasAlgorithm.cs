﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphsClassProject
{

    internal class DijkstrasAlgorithm
    {

        private readonly ParentGraph graph;
        public List<Vertex> Path { get; set; }
        private const int MaxVal = int.MaxValue;


        public DijkstrasAlgorithm(ParentGraph graph)
        {
            this.graph = graph;

            //path of nodes
            Path = new List<Vertex>();
        }



        public double DijskstrasShortestPath(Vertex source, Vertex target)
        {

            /*
            * how to determine if there is a cycle and target node cannot be reached
            */



            //return -1 if no path exists
            double shortestDist = -1.0;

            //vertex->corresponding structure
            Dictionary<Vertex, Dijkstra> vertexStructs =
                new Dictionary<Vertex, Dijkstra>();


            Dijkstra currNode = new Dijkstra(true, 0, source, source);  //intialize currNode to the source node
            Dijkstra targetNode = new Dijkstra(false, MaxVal, null, target);  //keep track of targetNode being false


            vertexStructs.Add(source, currNode); //add to dictionary

            while (currNode.Vertex != target) //!vertexStructs[target].sdFound
            {
                
                foreach (Vertex v in currNode.Vertex.Neighbors)
                {
                    Console.WriteLine("neighbor is " + v.Name);
                    //if newNode from this vertex doesn't exist
                    if (!vertexStructs.ContainsKey(v))
                    {
                        
                        if (v == target)
                        {
                            vertexStructs.Add(v, targetNode);
                        }
                        else
                        {
                            Dijkstra newNode = new Dijkstra(false, MaxVal, null, v);
                            vertexStructs.Add(v, newNode);
                        }
                        
                    }

                    Dijkstra currStruct = vertexStructs[v];

                    int newDistance = 0;

                    newDistance += vertexStructs[currNode.Vertex].DistanceFromStart + graph.GetWeight(currNode.Vertex, v);   //   not accessing parent here

                    Console.WriteLine(newDistance);
                    
                    if (newDistance < currStruct.DistanceFromStart)
                    {
                        //update parent and shortest dist of v
                        currStruct.Parent = currNode.Vertex;
                        currStruct.DistanceFromStart = newDistance;
                        vertexStructs.Remove(v);
                        vertexStructs.Add(v, currStruct);
                        
                        
                    }

                
                }

                

                //find shortest false node and set to currNode and true
                int shortestFalse = MaxVal;
                foreach (KeyValuePair<Vertex, Dijkstra> d in vertexStructs)
                {
             
                    if (!d.Value.SdFound && d.Value.DistanceFromStart < shortestFalse)
                    {
                        
                        currNode = d.Value;
                        shortestFalse = d.Value.DistanceFromStart;
                    }
                }

                if (shortestFalse == MaxVal)
                {
                    //all shortest paths have been found
                    throw new Exception("Selected vertices do not have a connection between them");
                }

                Console.WriteLine("shortest false is " + shortestFalse);

                currNode.SdFound = true;
                vertexStructs.Remove(currNode.Vertex);
                vertexStructs.Add(currNode.Vertex, currNode);
                Console.WriteLine("currNode is " + currNode.Vertex.Name);

            }

            shortestDist = currNode.DistanceFromStart;

            Console.WriteLine("Shortest distance is" + shortestDist);



            if (shortestDist != -1)
            {
                Vertex parent = currNode.Parent;
                Path.Add(parent);
                Console.WriteLine(currNode.Vertex.Name);
                Console.WriteLine(parent.Name);

                //create path - add parent vertex of node until reach node with source vertex
                while (parent != source)
                {
                    parent = vertexStructs[parent].Parent;
                    Path.Insert(0, parent);
                }

                Path.Add(currNode.Vertex);
                
                PrintVertexSequence(Path);
            }

            return shortestDist;
        }

        
        private void PrintVertexSequence(List<Vertex> path)
        {
            Console.WriteLine(path.Count);
            for (int i = 0; i < path.Count; i++)
            {
                Console.Write(path[i].Name);
            }
        }

        struct Dijkstra
        {
            internal bool SdFound { get; set; }
            internal int DistanceFromStart { get; set; }
            internal Vertex Parent { get; set; }
            internal Vertex Vertex { get; set; }

            public Dijkstra(bool sdFound, int distanceFromStart, Vertex parent, Vertex vertex)
            {
                this.SdFound = sdFound;
                this.DistanceFromStart = distanceFromStart;
                this.Parent = parent;
                this.Vertex = vertex;
            }

        }
    }
}