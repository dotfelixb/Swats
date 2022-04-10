import { CloseCircleOutlined } from "@ant-design/icons";
import { Button, Form, Input, Select } from "antd";
import React, { FC, useState } from "react";
import { ControlType, IPickerType } from "../interfaces";

interface IActionTag {
  key: string;
  value: string;
}


interface IWorkflowAction {
  id: number;
  onClick: (key: number) => void;
}

const actionTags: IPickerType[] = [
  { value: "forwardto", text: "Forward To", control: ControlType.Input },
  { value: "assignto", text: "Assign To", control: ControlType.Select },
  {
    value: "assigndepartment",
    text: "Assign Department",
    control: ControlType.Select,
  },
  { value: "assignteam", text: "Assign Team", control: ControlType.Select },
  { value: "changestatus", text: "Change Status", control: ControlType.Select },
  { value: "applysla", text: "Apply SLA", control: ControlType.Select },
];

const WorkflowAction: FC<IWorkflowAction> = ({ id, onClick }) => {
  const [control, setControl] = useState<ControlType>(ControlType.Input);

  const onChange = (value: any) => {
    const controltype = actionTags.find((f) => f.value === value);

    if (controltype !== null && controltype !== undefined) {
      setControl(controltype.control);
    }
  };

  return (
    <div className="grid grid-cols-1 sm:grid-cols-3 gap-5">
      <Form.Item name={`actionfrom-${id}`}>
        <Select showSearch onChange={onChange}>
          {actionTags.map((s) => (
            <Select.Option key={s.value} value={s.value}>
              {s.text}
            </Select.Option>
          ))}
        </Select>
      </Form.Item>
      <Form.Item name={`actionto-${id}`}>
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

export default WorkflowAction;
