import { useState } from 'react';
import { DecodeToken } from '../services/authentication-service';

const useToken = () => {
    const getToken = (): any => {
        const tokenString = localStorage.getItem('token');
        if (tokenString === null || tokenString === '') {
            return '';
        }
        const userToken: JwtBearerToken = { token: tokenString };
        return userToken.token;
    };

    const [token, setToken] = useState(getToken());

    const saveToken = (userToken: JwtBearerToken) => {
        localStorage.setItem('token', JSON.stringify(userToken.token));
        setToken(userToken.token);
    };

    let sureTokenValue: any = '';
    if (token) {
        sureTokenValue = token;
    }
    sureTokenValue.replaceAll('"', '');

    let sureDetail: UserDetails | undefined = { token: '', firstname: '', lastname: '', username: '', userref: '', roles: [] };
    if (sureTokenValue !== '') {
        sureDetail = DecodeToken({ token: sureTokenValue });
    }
    if (sureDetail) {
        sureDetail.token = sureDetail.token.replaceAll('"', '');
    }

    const [userDetail, setUserDetail] = useState(sureDetail);
    const deleteToken = () => {
        localStorage.removeItem('token');
        const emptyDetail = { token: '', firstname: '', lastname: '', username: '', userref: '', roles: [] };
        setUserDetail(emptyDetail);
        setToken('');
        location.reload();
    };

    return {
        setToken: saveToken,
        token,
        userDetail,
        logout: deleteToken,
    };
};

export default useToken;