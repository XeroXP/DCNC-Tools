﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TdfToXml
{
    [Serializable]
    [XmlRoot(ElementName = "Quests")]
    public class QuestTable
    {
        public struct Quest
        {
            [XmlAttribute("id")]
            public string Id;

            [XmlAttribute("missiontype")]
            public string MissionType;

            [XmlAttribute("tableindex")]
            public int TableIndex;

            [XmlAttribute("title")]
            public string Title;

            [XmlAttribute("introprompt")]
            public string IntroPrompt;
            [XmlAttribute("station")]
            public string StartStation;

            [XmlAttribute("completestation")]
            public string CompleteStation;
            [XmlAttribute("completeprompt")]
            public string CompletePrompt;

            [XmlAttribute("objective1")]
            public string Objective1;
            [XmlAttribute("objective2")]
            public string Objective2;
            [XmlAttribute("objective3")]
            public string Objective3;
            [XmlAttribute("objective4")]
            public string Objective4;
            [XmlAttribute("objective5")]
            public string Objective5;

            [XmlAttribute("exp")]
            public int Experience;
            [XmlAttribute("mito")]
            public int Mito;

            [XmlAttribute("reward1")]
            public string RewardItem1;
            [XmlAttribute("reward2")]
            public string RewardItem2;
            [XmlAttribute("reward3")]
            public string RewardItem3;
        }

        [XmlElement(ElementName = "Quest")]
        public List<Quest> QuestList = new List<Quest>();

        public void Load(string fileName)
        {
            var tdfFile = new TdfFile();
            tdfFile.Load(fileName);
            
            using (var reader = new BinaryReaderExt(new MemoryStream(tdfFile.ResTable)))
            {
                for (var row = 0; row < tdfFile.Header.Row; row++)
                {
                    var quest = new QuestTable.Quest();
                    quest.Id = reader.ReadUnicode();
                    quest.TableIndex = Convert.ToInt32(reader.ReadUnicode())-1;
                    quest.MissionType = reader.ReadUnicode();
                    //quest.Title = reader.ReadUnicode();
                    //quest.IntroPrompt = reader.ReadUnicode();

                    /*quest.Objective1 = reader.ReadUnicode();
                    quest.Objective2 = reader.ReadUnicode();
                    quest.Objective3 = reader.ReadUnicode();
                    quest.Objective4 = reader.ReadUnicode();
                    quest.Objective5 = reader.ReadUnicode();*/

                    // Total: 79, read until 74, from 2
                    for (int i = 3; i < 75; i++)
                    {
                        reader.ReadUnicode(); // Ignore everything.
                    }

                    quest.Experience = Convert.ToInt32(reader.ReadUnicode());
                    quest.Mito = Convert.ToInt32(reader.ReadUnicode());
                    quest.RewardItem1 = reader.ReadUnicode();
                    quest.RewardItem2 = reader.ReadUnicode();
                    quest.RewardItem3 = reader.ReadUnicode();
                    QuestList.Add(quest);
                    Console.WriteLine($"Quest added {quest.Id}");
                }
            }

            Console.WriteLine($"Quests added: {QuestList.Count}");
            Console.WriteLine($"Quest #1: {QuestList[0].Id}, Exp: {QuestList[0].Experience}, Mito: {QuestList[0].Mito}");
        }

        public void Save(string fileName)
        {
            var serializer = new XmlSerializer(typeof(QuestTable));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (var writer = new StreamWriter("Quests.xml"))
            {
                serializer.Serialize(writer, this, ns);
            }
        }
    }
}
