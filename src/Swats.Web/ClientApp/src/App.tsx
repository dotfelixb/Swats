import { FC } from "react";
import { Route, Routes } from "react-router-dom";
import "./site.css";
import "./custom.less";
import {
  ListAgents,
  Dashboard,
  NoMatch,
  Settings,
  Login,
  ListTickets,
  NewTicket,
  ViewTicket,
  NewAgent,
  ListDepartments,
  NewDepartment,
  ListTeams,
  NewTeam,
  ListTopics,
  NewTopic,
  ListHours,
  NewHour,
  ListTags,
  NewTag,
  ListTypes,
  NewType,
  ViewHour,
  ViewTopic,
  ViewTag,
} from "./pages";
import { MainLayout } from "./components";
import { AppProvider, AuthProvider, useAuth } from "./context";
import { LoadingOutlined } from "@ant-design/icons";

const Ready: FC = () => {
  const { browserLoaded } = useAuth();

  if (!browserLoaded) {
    return (
      <div>
        <LoadingOutlined />
      </div>
    );
  }

  return (
    <Routes>
      <Route path="/" element={<MainLayout />}>
        <Route index element={<Dashboard />} />
        <Route path="ticket">
          <Route index element={<ListTickets />} />
          <Route path="new" element={<NewTicket />} />
          <Route path=":id" element={<ViewTicket />} />
        </Route>
        <Route path="agent">
          <Route index element={<ListAgents />} />
          <Route path="new" element={<NewAgent />} />
        </Route>
        <Route path="admin">
          <Route index element={<Settings />} />
          <Route path="agent">
            <Route index element={<ListAgents />} />
            <Route path="new" element={<NewAgent />} />
          </Route>
          <Route path="department">
            <Route index element={<ListDepartments />} />
            <Route path="new" element={<NewDepartment />} />
          </Route>
          <Route path="team">
            <Route index element={<ListTeams />} />
            <Route path="new" element={<NewTeam />} />
          </Route>
          <Route path="helptopic">
            <Route index element={<ListTopics />} />
            <Route path="new" element={<NewTopic />} />
            <Route path=":id" element={<ViewTopic />} />
          </Route>
          <Route path="tickettype">
            <Route index element={<ListTypes />} />
            <Route path="new" element={<NewType />} />
          </Route>
          <Route path="businesshour">
            <Route index element={<ListHours />} />
            <Route path="new" element={<NewHour />} />
            <Route path=":id" element={<ViewHour />} />
          </Route>
          <Route path="tag">
            <Route index element={<ListTags />} />
            <Route path="new" element={<NewTag />} />
            <Route path=":id" element={<ViewTag />} />
          </Route>
        </Route>
      </Route>
      <Route path="/login" element={<Login />} />
      <Route path="*" element={<NoMatch />} />
    </Routes>
  );
};

const App: FC = () => (
  <AuthProvider>
    <AppProvider>
      <Ready />
    </AppProvider>
  </AuthProvider>
);

export default App;
