import React, { FC } from "react";

interface IDataColumn {
  column: { title: string }[];
  key: string;
}

interface IDataTable {
  columns: IDataColumn[];
}

const DataTable: FC<IDataTable> = ({ columns, children }) => {
  const renderColumn = (
    <thead className="datatable">
      <tr className="text-left px-3 bg-indigo-50">
        {columns.map((c) => {
          return (
            <th key={c.key} className="px-3 py-3 font-semibold">
              {c.column.map((cc) => (
                <div key={cc.title}>{cc.title}</div>
              ))}
            </th>
          );
        })}
      </tr>
    </thead>
  );

  return (
    <div className="bg-white border border-gray-200 rounded-sm">
      {/* filter fields */}
      <div className="grid grid-cols-5 gap-4 px-5 py-5">
        <div>
          <div className="w-full">
            <input className="form-input-search" placeholder="Name" />
          </div>
        </div>
        <div>
          <div className="w-full">
            <input className="form-input-search" placeholder="Name" />
          </div>
        </div>
      </div>

      {/* filter tags */}
      <div className="w-full px-5 pb-3">
        <span className="text-xs rounded bg-indigo-500 text-white px-2 py-1">
          Filter
        </span>
        <span className="text-xs rounded bg-indigo-500 text-white px-2 py-1">
          Name
        </span>
      </div>

      {/* table */}
      <div className="w-full border-t-2 border-gray-50">
        <table className="w-full table-auto">
          {renderColumn}
          <tbody>{children}</tbody>
        </table>
      </div>
    </div>
  );
};

export default DataTable;
