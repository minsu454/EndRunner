using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using SqlCipher4Unity3D;
using System.Reflection;
using System.Linq;

public class DataService
{
    public string databaseName = "RollingRunnerDatabase.db";

    private SQLiteConnection connection;
    private Dictionary<Type, Dictionary<int, IKeyTableData>> tableDic;

    public void InitDataService() {
        InitDB();
        InitDataForPlay();
    }

    /// <summary>
    /// DB파일 경로 확인 및 복사해주는 함수
    /// </summary>
    private void InitDB()
    {
        string streamingAssetsPath = "";

#if UNITY_EDITOR
        streamingAssetsPath = string.Format("Assets/StreamingAssets/{0}", databaseName);
        databaseName = string.Format("Assets/{0}", databaseName);

#elif UNITY_ANDROID || UNITY_IOS
        streamingAssetsPath = string.Format("{0}/{1}", Application.streamingAssetsPath, databaseName);
        databaseName = string.Format("{0}/{1}", Application.persistentDataPath, databaseName);
#endif

        if (File.Exists(databaseName) == false)
        {
#if UNITY_EDITOR || UNITY_IOS
            File.Copy(streamingAssetsPath, databaseName);

#elif UNITY_ANDROID
            WWW loadDb = new WWW(streamingAssetsPath);
            while (loadDb.isDone == false) { }
            File.WriteAllBytes(databaseName, loadDb.bytes);
            loadDb.Dispose();
            loadDb = null;

#endif

        }
    }

    /// <summary>
    /// DB데이터 읽고 Dict에 저장해주는 함수
    /// </summary>
    private void InitDataForPlay()
    {
        if (connection == null)
        {
            connection = new SQLiteConnection(databaseName, "ms9935");
        }

        tableDic = new Dictionary<Type, Dictionary<int, IKeyTableData>>();
        for (int i = 0; i < Table.readOnlyTableTypeArray.Length; i++)
        {
            tableDic.Add(Table.readOnlyTableTypeArray[i], new Dictionary<int, IKeyTableData>());
        }
        for (int i = 0; i < Table.writableTableTypeArray.Length; i++)
        {
            tableDic.Add(Table.writableTableTypeArray[i], new Dictionary<int, IKeyTableData>());
        }

        MethodInfo loadDataMethod = this.GetType().GetMethod("LoadData", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var type in tableDic.Keys)
        {
            MethodInfo loadDataGenericMethod = loadDataMethod.MakeGenericMethod(type);
            loadDataGenericMethod.Invoke(this, new object[] { });
        }
    }

    public void LoadData<T>() where T : IKeyTableData, new() {
        var tableDataList = connection.Table<T>().ToList();
        for (int i = 0; i < tableDataList.Count; i++) {
            tableDic[typeof(T)].Add(tableDataList[i].GetTableId(), tableDataList[i]);
        }

        T table = new T();
        IInsertableTableData insertable = table as IInsertableTableData;
        if (insertable != null) {
            if (tableDic[typeof(T)].Count > 0)
            {
                insertable.Max = tableDic[typeof(T)].Keys.Max();
            }
            else {
                insertable.Max = -1;
            }
        }
    }

    // 해당 테이블의 id값의 모든 컬럼 데이터를 리턴
    public T GetData<T>(int id) where T : IKeyTableData
    {
        Dictionary<int, IKeyTableData> dataDic = tableDic[typeof(T)];
        if (dataDic.ContainsKey(id))
        {
            return (T)dataDic[id];
        }
        Debug.LogError("GetData Error : No Data " + typeof(T).ToString() + ", id :" + id);
        return default(T);
    }

    // 해당 테이블의 모든 데이터를 리스트로 리턴
    public List<T> GetDataList<T>() where T : IKeyTableData
    {
        if (tableDic.ContainsKey(typeof(T)))
        {
            return tableDic[typeof(T)].Values.Cast<T>().ToList();
        }
        Debug.LogError("GetDataList Error : No Table -" + typeof(T).ToString());
        return default(List<T>);
    }


    // 테이블에 값 넣기. 리턴 값은 추가된 데이터의 아이디
    public int InsertData<T>(T table) where T : IKeyTableData, IInsertableTableData
    {
        table.Max++;
        int nextId = table.Max;
        table.SetTableId(nextId);
        tableDic[typeof(T)].Add(nextId, table);
        connection.Insert(table);
        return nextId;
    }

    // 테이블 값 수정하기 리턴값은 성공(1) 실패(0)
    public int UpdateData<T>(T table) where T : IKeyTableData
    {
        tableDic[typeof(T)][table.GetTableId()] = table;
        return connection.Update(table);
    }

    // 테이블 데이터 삭제
    public void DeleteData<T>(int id) where T : IKeyTableData
    {
        tableDic[typeof(T)].Remove(id);
        connection.Delete<T>(id);
    }

    //List에 넘어오는 모든 데이터 바꿔주기
    public void UpdateDataAll<T>(List<T> tableList) where T : IKeyTableData
    {
        for (int i = 0; i < tableList.Count; i++)
        {
            tableDic[typeof(T)][tableList[i].GetTableId()] = tableList[i];
        }
        connection.UpdateAll(tableList);
    }

    //List로 넘어오는 모든 데이터 삭제
    public void DeleteDataAll<T>(List<T> tableList) where T : IKeyTableData
    {
        for (int i = 0; i < tableList.Count; i++)
        {
            tableDic[typeof(T)].Remove(tableList[i].GetTableId());
        }
        connection.DeleteAll(tableList);
    }

    //TextTable 받아오기
    public string GetText(int id) {
        var dataDic = tableDic[typeof(Table.TextTable)];
        if (dataDic.ContainsKey(id))
        {
            Table.TextTable textData = (Table.TextTable)dataDic[id];
            var saveData = GetData<Table.SaveTable>(0);
            if (saveData.language == "None")
            {
                switch (Application.systemLanguage)
                {
                    case SystemLanguage.Korean:
                        return textData.kor;
                    default:
                        return textData.eng;
                }
            }
            else {
                switch (saveData.language)
                {
                    case "kor":
                        return textData.kor;
                    default:
                        return textData.eng;
                }
            }
        }
        else {
            return "[TextError]" + id;
        }
    }
}
