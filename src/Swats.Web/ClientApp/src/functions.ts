
import { ILogin, ILoginResult } from "./interfaces";

// const BASE_URL = "https://localhost:7268/methods/"; //live

let headers = new Headers();
headers.append("content-type", "application/json");

export const login = async ({username, password, remember}:ILogin) : Promise<ILoginResult | null> => {
  const body = new FormData();
  body.append("username", username);
  body.append("password", password);

  const f = await fetch("methods/auth.login", {
    method: "POST",
    body
  })

  const result = await f.json();

  return result;
}