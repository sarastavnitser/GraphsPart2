﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;



namespace GraphsClassProject 
{
    class Digraph
    {
        public List<Vertex> Vertices { get; set; }

        public Digraph()
        {
            Vertices = new List<Vertex>();
        }

        public Digraph(List<Vertex> nodes)
        {
            Vertices = nodes;
        }

        public void AddNode(Vertex node)
        {
            Vertices.Add(node);
        }

        public bool LoadGraph(DataSet dataSet)
        {
            bool retVal = true;

            // edge table: initialNode, terminalNode, weight (should be 1)

            var nrEdges = dataSet.Tables["Edges"].Rows.Count;
            for (int row = 1; row < nrEdges; ++row)
            {
                // check initial node
                String initialNode = (String)dataSet.Tables["Edges"].Rows[row].ItemArray[0];
                String terminalNode = (String)dataSet.Tables["Edges"].Rows[row].ItemArray[1];

                int initialIndex = Vertices.FindIndex(item => initialNode.Equals(item.Name));
                int terminalIndex = Vertices.FindIndex(item => terminalNode.Equals(item.Name));

                Vertex initial = initialIndex < 0 ? new Vertex(initialNode)
                                                : Vertices[initialIndex];
                Vertex terminal = terminalIndex < 0 ? new Vertex(terminalNode)
                                                : Vertices[terminalIndex];

                if (initialIndex < 0 && terminalIndex < 0)
                {
                    // neither exist - create both, add edge between them with weight = 1
                    Vertices.Add(initial);
                    Vertices.Add(terminal);
                }
                else if (initialIndex < 0 && terminalIndex > -1)
                {
                    // initial doesn't exist, create and add edge between it and terminal with weight = 1
                    Vertices.Add(initial);
                }
                else if (initialIndex > -1 && terminalIndex < 0)
                {
                    // terminal doesn't exist, create and add edge between initial and it with weight = 1
                    Vertices.Add(terminal);
                }
                // if they both already exist, no need to add anything
                initial.AddEdge(terminal, 1);
            }

            return retVal;
        }

        public bool LoadVertices(String fileName)
        {

            bool retVal = true;

            try
            {
                using (TextReader reader = new StreamReader(fileName))
                {
                    // assume 1 line per vertex, first element is name of vertex, rest is name of neighbors (unidirectional)
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Trim().Length == 0) continue;

                        string[] vertices = line.Split();

                        if (vertices.Length > 0)
                        {
                            // TODO add check that the vertex names are not repeated  
                            Vertex v = new Vertex(vertices[0]);
                            Vertices.Add(v);
                            string vertexName = vertices[0];
                            for (int eix = 1; eix < vertices.Length; ++eix)
                            {
                                Vertex nbr = new Vertex(vertices[eix]);
                                v.AddEdge(nbr, 1);
                            }
                        }


                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                retVal = false;
            }


            return retVal;
        }


    }
}
