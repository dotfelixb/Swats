import {Checkbox, DatePicker, Form} from "antd";
import dayjs from "dayjs";
import {FC, useState} from "react";
import {IOpenHour} from "../interfaces";

interface IOpenHourItem {
    data: IOpenHour;
    hoursList: IOpenHour[];
    setHoursList: React.Dispatch<React.SetStateAction<IOpenHour[]>>;
}

const OpenHourItem: FC<IOpenHourItem> = ({data, hoursList, setHoursList}) => {
    const [newData, setNewData] = useState(data);
    const [dayDisabled, setDayDisabled] = useState(true);
    const [dateDisabled, setDateDisabled] = useState(true);

    const onChange = (e: any) => {
        // remove this hour
        const filteredList = hoursList.filter((f) => f.name !== data.name);
        // create a new hour
        const newItem: IOpenHour = {
            ...newData,
            enabled: e.target.checked,
        };

        // set hours
        setNewData(newItem);
        setHoursList([...filteredList, newItem]);

        setDayDisabled(!e.target.checked);
        setDateDisabled(!e.target.checked);
    };

    const onDayChange = (e: any) => {
        // remove this hour
        const filteredList = hoursList.filter((f) => f.name !== data.name);
        // create a new hour
        const newItem: IOpenHour = {
            ...newData,
            fullDay: e.target.checked,
            fromTime: undefined,
            toTime: undefined,
        };

        // set hours
        setNewData(newItem);
        setHoursList([...filteredList, newItem]);

        setDateDisabled(e.target.checked);
    };

    const onFromChange = (e: any) => {
        // remove this hour
        const filteredList = hoursList.filter((f) => f.name !== data.name);
        // create a new hour
        const newItem: IOpenHour = {
            ...newData,
            fromTime: dayjs(e).format(),
        };

        // set hours
        setNewData(newItem);
        setHoursList([...filteredList, newItem]);
    };

    const onToChange = (e: any) => {
        // remove this hour
        const filteredList = hoursList.filter((f) => f.name !== data.name);
        // create a new hour
        const newItem: IOpenHour = {
            ...newData,
            toTime: dayjs(e).format(),
        };

        // set hours
        setNewData(newItem);
        setHoursList([...filteredList, newItem]);
    };

    return (
        <div className="grid grid-cols-1 sm:grid-cols-4 gap-5">
            <Form.Item name={data.name} valuePropName="checked">
                <Checkbox onChange={onChange}>{data.name}</Checkbox>
            </Form.Item>
            <Form.Item name={`${data.name}hour`} valuePropName="checked">
                <Checkbox onChange={onDayChange} disabled={dayDisabled}>
                    Open 24 Hours
                </Checkbox>
            </Form.Item>
            <Form.Item name={`${data.name}from`} valuePropName="checked">
                <DatePicker
                    placeholder="From"
                    onChange={onFromChange}
                    picker="time"
                    minuteStep={10}
                    secondStep={10}
                    disabled={dateDisabled}
                />
            </Form.Item>
            <Form.Item name={`${data.name}to`} valuePropName="checked">
                <DatePicker
                    placeholder="To"
                    onChange={onToChange}
                    picker="time"
                    minuteStep={10}
                    secondStep={10}
                    disabled={dateDisabled}
                />
            </Form.Item>
        </div>
    );
};

export default OpenHourItem;