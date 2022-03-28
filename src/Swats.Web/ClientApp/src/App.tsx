import React, { FC } from "react";
import { Route, Routes } from "react-router-dom";
import "./custom.less";
import "./app.css";
import { Agent, Dashboard, NoMatch, Settings } from "./pages";
import { MainLayout } from "./components";
import { ListTicket, NewTicket, ViewTicket } from "./pages/Ticket";

const App: FC = () => (
  <MainLayout>
    <Routes>
      <Route index element={<Dashboard />} />
      <Route path="ticket" >
        <Route index element={<ListTicket />} />
        <Route path="new" element={<NewTicket />} />
        <Route path=":ticketId" element={<ViewTicket />} />
      </Route>
      <Route path="/agent" element={<Agent />} />
      <Route path="/admin" element={<Settings />} />
      <Route path="*" element={<NoMatch />} />
    </Routes>
  </MainLayout>
);

export default App;
