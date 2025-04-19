using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKeyTableData {
    int GetTableId();
}

public interface IInsertableTableData {
    void SetTableId(int id);
    int Max {
        get;
        set;
    }
}