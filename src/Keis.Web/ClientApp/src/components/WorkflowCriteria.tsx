import { CloseCircleOutlined } from "@ant-design/icons";
import { Button, Form, Input, Select } from "antd";
import React, { FC, useState } from "react";
import { ControlType, IPickerType } from "../interfaces";

interface IWorkflowCriteria {
  id: number;
  onClick: (key: number) => void;
}

const criteriaTags: IPickerType[] = [
  { value: "subject", text: "Subject", control: ControlType.Input },
  { value: "department",  text: "Department", control: ControlType.Select },
  { value: "team", text: "Team", control: ControlType.Select },
  { value: "status", text: "Status", control: ControlType.Select },
  { value: "priority", text: "Priority", control: ControlType.Select },
];

const WorkflowCriteria: FC<IWorkflowCriteria> = ({ id, onClick }) => {
  const [control, setControl] = useState<ControlType>(ControlType.Input);

  const onChange = (value: any) => {
    const controltype = criteriaTags.find((f) => f.value === value);

    if (controltype !== null && controltype !== undefined) {
      setControl(controltype.control);
    }
  };
  
  return (
    <div className="grid grid-cols-1 sm:grid-cols-4 gap-5">
      <Form.Item name={`when-${id}`}>
        <Select showSearch onChange={onChange}>
          {criteriaTags.map((s) => (
            <Select.Option key={s.value} value={s.value}>
              {s.text}
            </Select.Option>
          ))}
        </Select>
      </Form.Item>
      <Form.Item name={`match-${id}`}>
        <Select>
          <Select.Option value="1">Equals</Select.Option>
          <Select.Option value="2">Contains</Select.Option>
        </Select>
      </Form.Item>
      <Form.Item name={`this-${id}`}>
        {control === ControlType.Input ? <Input /> : <Select></Select>}
      </Form.Item>
      <Form.Item>
        <Button
          type="link"
          danger
          size="small"
          onClick={() => onClick(id)}
          icon={<CloseCircleOutlined />}
        />
      </Form.Item>
    </div>
  );
};

export default WorkflowCriteria;
