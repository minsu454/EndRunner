using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Scripting;
using SQLite.Attribute;

public class Table
{
    public static Type[] readOnlyTableTypeArray = new Type[] {
        typeof(TextTable),
        typeof(ChallengeTable),
        typeof(EnemySpawnTable)
    };

    public static Type[] writableTableTypeArray = new Type[] {
        typeof(SaveTable),
        typeof(PlayerCharacterTable),
        typeof(PlayerAccessoryTable),
        typeof(PlayerChallengeTable)
    };

    [Preserve, Serializable]
    public class SaveTable : IKeyTableData, IInsertableTableData {

        public void SetTableId(int id)
        {
            this.id = id;
        }
        public static int max;
        [Ignore]
        public int Max
        {
            get { return max; }
            set { max = value; }
        }

        public int GetTableId() { return id; }

        [NotNull, PrimaryKey, Unique]
        public int id { get; set; }
        public string language { get; set; }
        public int high_score_easy { get; set; }
        public int high_score_hard { get; set; }
        public int play_count { get; set; }
        public int item_count { get; set; }
        public int coin { get; set; }
        public string bombreducedcooldowntime { get; set; }
        public string barrier { get; set; }
        public string hyperrun { get; set; }
        public string pick_character { get; set; }
        public string pick_hat { get; set; }
        public string pick_eye { get; set; }
        public string pick_tie { get; set; }

    }

    [Preserve, Serializable]
    public class TextTable : IKeyTableData
    {

        public int GetTableId() { return id; }

        [NotNull, PrimaryKey, Unique]
        public int id { get; set; }
        public int text_id { get; set; }
        public string kor { get; set; }
        public string eng { get; set; }

    }

    [Preserve, Serializable]
    public class PlayerCharacterTable : IKeyTableData, IInsertableTableData
    {

        public void SetTableId(int id)
        {
            this.id = id;
        }
        public static int max;
        [Ignore]
        public int Max
        {
            get { return max; }
            set { max = value; }
        }

        public int GetTableId() { return id; }

        [NotNull, PrimaryKey, Unique]
        public int id { get; set; }
        public string name { get; set; }
        public string my_character { get; set; }

    }

    [Preserve, Serializable]
    public class PlayerAccessoryTable : IKeyTableData, IInsertableTableData
    {

        public void SetTableId(int id)
        {
            this.id = id;
        }
        public static int max;
        [Ignore]
        public int Max
        {
            get { return max; }
            set { max = value; }
        }

        public int GetTableId() { return id; }

        [NotNull, PrimaryKey, Unique]
        public int id { get; set; }
        public string name { get; set; }
        public string accessorytype { get; set; }
        public string my_accessories { get; set; }

    }

    [Preserve, Serializable]
    public class PlayerChallengeTable : IKeyTableData, IInsertableTableData
    {

        public void SetTableId(int id)
        {
            this.id = id;
        }
        public static int max;
        [Ignore]
        public int Max
        {
            get { return max; }
            set { max = value; }
        }

        public int GetTableId() { return id; }

        [NotNull, PrimaryKey, Unique]
        public int id { get; set; }
        public string clear_challenge { get; set; }

    }

    [Preserve, Serializable]
    public class ChallengeTable : IKeyTableData
    {

        public int GetTableId() { return id; }

        [NotNull, PrimaryKey, Unique]
        public int id { get; set; }
        public int text_id { get; set; }
        public int value { get; set; }
        public string reward_json { get; set; }

    }

    [Preserve, Serializable]
    public class EnemySpawnTable : IKeyTableData
    {

        public int GetTableId() { return id; }

        [NotNull, PrimaryKey, Unique]
        public int id { get; set; }
        public string Black { get; set; }
        public string Yellow { get; set; }
        public string Green { get; set; }
        public string Red { get; set; }
        public string Laser { get; set; }
    }
}
