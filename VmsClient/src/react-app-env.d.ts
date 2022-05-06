/// <reference types="react-scripts" />
interface tokenHandler {
    setToken: Function
}

interface RegisterModel {
    email: any;
    password: any;
}

interface JwtBearerToken {
    token: string;
}

interface UserDetails {
    token: string;
    firstname: string;
    lastname: string;
    username: string;
    userref: string;
    roles: Array<string>;
}

interface UserPassing {
    firstname: string;
    lastname: string;
    id: string;
    token: string;
    roles: Array<string>;
    email: string;
  }
  