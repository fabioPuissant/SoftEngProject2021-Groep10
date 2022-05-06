import jwtDecode from 'jwt-decode';

export const DecodeToken = (jwtToken: JwtBearerToken) => {
    const token: string = jwtToken.token;
    if (token === 'undefined') {
        return undefined;
    }
    const decoded: UserDetails = jwtDecode<UserDetails>(token);
    decoded.roles = Array.isArray(decoded.roles) ? decoded.roles :  [decoded.roles]
    decoded.token = token;
    return decoded;
};

type FailureCallback = (e: any) => void;

export const LoginUser = async (credential: RegisterModel, failed: FailureCallback) => {
    const data = await fetch('https://localhost:44320/api/Accounts/Login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accecpt': 'application/json',
        },
        body: JSON.stringify(credential)
    }).then(res => {
        if (res.status === 401) {
            return undefined;
        }
        return res.json();
    }).catch(e => failed(e));
    return data;
};