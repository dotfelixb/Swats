import { Breadcrumb } from "antd";
import dayjs from "dayjs";
import { FC, useCallback, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { PageView } from "../../components";
import { useApp, useAuth } from "../../context";
import { IFetchWorkflow, IListResult, ISingleResult, IWorkflowEvent } from "../../interfaces";

interface IViewWorkflow {}

const ViewWorkflow: FC<IViewWorkflow> = () => {
  const { user } = useAuth();
  const { get, dateFormats } = useApp();
  const { id } = useParams();
  const [workflow, setWorkflow] = useState<IFetchWorkflow>();
  const [workflowEventList, setWorkflowEventList] = useState<IWorkflowEvent[]>(
    []
  );
  const [eventList , setEventList] = useState<IWorkflowEvent[]>([]);

  const loadEvent = useCallback(async () => {
    const g: Response = await get(`methods/workflow.event`);

    if (g != null) {
      const d: IListResult<IWorkflowEvent[]> = await g.json();

      if (g.status === 200 && d.ok) {
        setWorkflowEventList(d.data);
      } else {
        // TODO: display error to user
      }
    }
  }, [get]);

  const load = useCallback(async () => {
    const g: Response = await get(`methods/workflow.get?id=${id}`);
    const d: ISingleResult<IFetchWorkflow> = await g.json();

    if (g.status === 200 && d.ok) {
      setWorkflow(d.data);
    } else {
      // TODO: display error to user
    }
  }, [get, id]);

  useEffect(() => {
    if (user != null && user.token && id) {
      load();
      loadEvent();
    }
  }, [user, id, get, load, loadEvent]);

  useEffect (()=>{
    const els = workflowEventList.filter(e => workflow?.events.includes(e.type));
    setEventList(els);
  }, [workflow, workflowEventList])


  const Breadcrumbs: FC = () => (
    <Breadcrumb separator="/">
      <Breadcrumb.Item>
        <Link to="/">Home</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        <Link to="/admin">Admin</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>
        <Link to="/admin/workflow">Workflows</Link>
      </Breadcrumb.Item>
      <Breadcrumb.Item>View Workflow</Breadcrumb.Item>
    </Breadcrumb>
  );

  return (
    <PageView
      title={workflow?.name ?? "View Workflow"}
      breadcrumbs={<Breadcrumbs />}
    >
      <div className="w-full flex flex-row ">
        <div style={{ width: "220px" }} className="">
          <div className="pr-2">
            <div className="bg-gray-200 rounded-sm w-28 h-28"></div>
          </div>
          <ul className="edit-sidebar py-5">
            <li>
              <div>Created By</div>
              <div>{workflow?.createdByName ?? ""}</div>
            </li>
            <li>
              <div>Created At</div>
              <div>
                {dayjs(workflow?.createdAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
            <li>
              <div>Updated By</div>
              <div>{workflow?.updatedByName ?? ""}</div>
            </li>
            <li>
              <div>Updated At</div>
              <div>
                {dayjs(workflow?.updatedAt ?? new Date()).format(
                  dateFormats.shortDateFormat
                )}
              </div>
            </li>
          </ul>
        </div>
        <div className="w-full bg-white border border-gray-200 rounded-sm px-10 py-10">
        <div className="grid grid-cols-1 gap-5 py-4">
            <div>
              <label className="form-label">Events</label>
              <div className="form-data">{eventList?.map(e => <span key={e.type}>{e.name} {" "}</span>)}</div>
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-3">
            <div>
              <label className="form-label">Priority</label>
              <div className="form-data">{workflow?.priority}</div>
            </div>

            <div>
              <label className="form-label">Status</label>
              <div className="form-data">{workflow?.status}</div>
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-4 gap-3"></div>

          <div className="grid grid-cols-1 gap-5 py-4">
            <div>
              <label className="form-label">Note</label>
              <div className="form-data">{workflow?.note ?? "N/A"}</div>
            </div>
          </div>
        </div>
      </div>
    </PageView>
  );
};

export default ViewWorkflow;
