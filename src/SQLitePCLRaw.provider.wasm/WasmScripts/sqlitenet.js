let fileMap = {};
let mountDone = false;

var SQLiteNative;

require(
    ["sqlite3"],
    instance => {
        SQLiteNative = instance;

        SQLiteNative.Database.enablePersistence("/local");
    }
)();

class SQLiteWasm {
    static sqliteLibVersionNumber() {

        SQLiteWasm.ensureInitialized();
        return SQLiteNative.Database.libversion_number();
    }

    static ensureInitialized() {
        if (!SQLiteNative) {
            SQLiteNative = require('sqlite3');
        }
    }

    static sqlite3_open(fileName) {

        var r2 = SQLiteNative.Database.open(fileName, null, { mode: "rwc", cache: "private" });

        fileMap[r2.pDB] = fileName;

        return `${r2.Result};${r2.pDB}`;
    }

    static sqlite3_open_v2(fileName, flags, vfs)
    {
        var r2 = SQLiteNative.Database.open_v2(fileName, null, flags, vfs);

        fileMap[r2.pDB] = fileName;

        return `${r2.Result};${r2.pDB}`;
    }

    static sqliteClose2(pDb) {
        var result = SQLiteNative.Database.close_v2(pDb, false);

        if (result.Data) {
            FS.writeFile(fileMap[pDb], result.Data, { encoding: 'binary', flags: "w" });
        }

        return result.Result;
    }

    static sqlitePrepare2(pDb, query) {

        var stmt = SQLiteNative.Database.prepare2(pDb, query);

        return `${stmt.Result};${stmt.pStatement};${stmt.TailIndex}`;
    }

    static sqliteChanges(dbId) {
        return SQLiteNative.Database.changes(dbId);
    }

    static sqliteErrMsg(dbId) {
        return SQLiteNative.Database.errmsg(dbId);
    }

    static sqlite3_busy_timeout(dbId, timeout) {
        return SQLiteNative.Database.busy_timeout(dbId, timeout);
    }

    static sqliteLastInsertRowid(dbId) {
        return SQLiteNative.Database.last_insert_rowid(dbId);
    }

    static sqlite3_errcode(dbId) {
        return SQLiteNative.Database.errcode(dbId);
    }

    static sqlite3_extended_result_codes(dbId, onoff) {
        return SQLiteNative.Database.sqlite3_extended_result_codes(dbId, onoff);
    }

    static sqlite3_extended_errcode(dbId) {
        return SQLiteNative.Database.extended_errcode(dbId);
    }

    static sqliteStep(pStatement) {
        return SQLiteNative.Database.step(pStatement);
    }

    static sqliteReset(pStatement) {
        return SQLiteNative.Database.reset(pStatement);
    }

    static sqliteFinalize(pStatement) {
        var res = SQLiteNative.Database.finalize(pStatement);

        return res;
    }

    static sqliteColumnType(pStatement, index) {
        return SQLiteNative.Database.column_type(pStatement, index);
    }

    static sqliteColumnString(pStatement, index) {
        return SQLiteNative.Database.column_text(pStatement, index);
    }

    static sqliteColumnInt(pStatement, index) { 
        return SQLiteNative.Database.column_int(pStatement, index);
    }

    static sqlite3_column_int64(pStatement, index, pResultData) {
        var data = SQLiteNative.Database.column_int64ptr(pStatement, index);

        for (var i = 0; i < 8; i++) {
            Module.HEAPU8[pResultData + i] = data[i];
        }
    }

    static sqlite3_column_double(pStatement, index) {
        return SQLiteNative.Database.column_double(pStatement, index);
    }

    static sqlite3_column_blob(pStatement, index, pData, length) {
        var data = SQLiteNative.Database.column_blob(pStatement, index);

        for (var i = 0; i < length; i++) {
            Module.HEAPU8[pData + i] = data[i]; 
        }
    }

    static sqlite3_column_bytes(pStatement, index) {
        return SQLiteNative.Database.column_bytes(pStatement, index);
    }

    static sqliteColumnCount(pStatement) {
        return SQLiteNative.Database.column_count(pStatement);
    }

    static sqliteColumnName(pStatement, index) {
        return SQLiteNative.Database.column_name(pStatement, index);
    }

    static sqliteBindText(pStatement, index, val) {
        return SQLiteNative.Database.bind_text(pStatement, index, val);
    }

    static sqliteBindNull(pStatement, index) {
        return SQLiteNative.Database.bind_null(pStatement, index);
    }

    static sqliteBindInt(pStatement, index, value) {
        return SQLiteNative.Database.bind_int(pStatement, index, value);
    }

    static sqliteBindInt64(pStatement, index, pValue) {

        var valueArray = new Uint8Array(8);
        for (var i = 0; i < 8; i++) {
            valueArray[i] = Module.HEAPU8[pValue + i];
        }

        return SQLiteNative.Database.bind_int64(pStatement, index, valueArray);
    }

    static sqliteBindDouble(pStatement, index, value) {
        return SQLiteNative.Database.bind_double(pStatement, index, value);
    }

    static sqlite3_bind_blob(pStatement, index, pValue, length) {

        var valueArray = new Uint8Array(length);
        for (var i = 0; i < length; i++) {
            valueArray[i] = Module.HEAPU8[pValue + i];
        }

        return SQLiteNative.Database.bind_blob(pStatement, index, valueArray, length);
    }

    static sqlite3_bind_parameter_index(pStatement, name) {
        return SQLiteNative.Database.bind_parameter_index(pStatement, name);
    }

    static sqlite3_bind_parameter_name(pStatement, index) {
        return SQLiteNative.Database.bind_parameter_name(pStatement, index);
    }

    static sqlite3_bind_parameter_count(pStatement) {
        return SQLiteNative.Database.bind_parameter_count(pStatement);
    }

    static sqlite3_stmt_readonly(pStatement) {
        return SQLiteNative.Database.stmt_readonly(pStatement);
    }

    static sqlite3_db_filename(pDB, zDBName) {
        return SQLiteNative.Database.db_filename(pDB, zDBName);
    }
}
