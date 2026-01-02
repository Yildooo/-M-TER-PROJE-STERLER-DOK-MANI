using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BasitNotTakip.Models;

namespace BasitNotTakip.Data
{
    public static class Database
    {
        private static readonly string DataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BasitNotTakip");
        private static readonly string DataFile = Path.Combine(DataFolder, "data.json");

        public static List<Not> Notlar { get; private set; } = new List<Not>();
        public static List<Gorev> Gorevler { get; private set; } = new List<Gorev>();

        private class DbDump
        {
            public List<Not> Notlar { get; set; }
            public List<Gorev> Gorevler { get; set; }
        }

        public static void Load()
        {
            try
            {
                if (!Directory.Exists(DataFolder)) Directory.CreateDirectory(DataFolder);
                if (!File.Exists(DataFile))
                {
                    Save();
                    return;
                }

                var json = File.ReadAllText(DataFile);
                if (string.IsNullOrWhiteSpace(json)) return;

                var dump = JsonSerializer.Deserialize<DbDump>(json);
                Notlar = dump?.Notlar ?? new List<Not>();
                Gorevler = dump?.Gorevler ?? new List<Gorev>();
            }
            catch
            {
                Notlar = new List<Not>();
                Gorevler = new List<Gorev>();
            }
        }

        public static void Save()
        {
            try
            {
                if (!Directory.Exists(DataFolder)) Directory.CreateDirectory(DataFolder);
                var dump = new DbDump { Notlar = Notlar, Gorevler = Gorevler };
                var json = JsonSerializer.Serialize(dump, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(DataFile, json);
            }
            catch
            {
                // ignore errors for simplicity
            }
        }

        public static void AddNot(Not n)
        {
            var nextId = Notlar.Count == 0 ? 1 : Notlar[^1].Id + 1;
            n.Id = nextId;
            Notlar.Add(n);
            Save();
        }

        public static void RemoveNot(int id)
        {
            Notlar.RemoveAll(x => x.Id == id);
            Save();
        }

        public static void AddGorev(Gorev g)
        {
            var nextId = Gorevler.Count == 0 ? 1 : Gorevler[^1].Id + 1;
            g.Id = nextId;
            Gorevler.Add(g);
            Save();
        }

        public static void RemoveGorev(int id)
        {
            Gorevler.RemoveAll(x => x.Id == id);
            Save();
        }

        public static void ToggleGorev(int id)
        {
            var g = Gorevler.Find(x => x.Id == id);
            if (g == null) return;
            g.Tamamlandi = !g.Tamamlandi;
            Save();
        }
    }
}
