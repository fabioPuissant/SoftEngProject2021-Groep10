import './LoginView.css';
import React, { useRef, useState } from 'react';
import Card from '../../components/Card/Card';
import CardBody from '../../components/Card/CardBody';
import '../../../node_modules/bootstrap/dist/css/bootstrap.min.css';
import CardHeader from '../../components/Card/CardHeader';
import { LoginUser } from '../../services/authentication-service';
// username = "xxxx@example.com",
// password = "admin123"
interface TokenCall {
    tokenCallback: Function;
}

export default function LoginView({ tokenCallback }: TokenCall) {
    const [username, setUserName] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState(false);
    const [loading, setLoading] = useState(false);
    // const history = useHistory()

    const handleSubmit = async (e: any) => {
        e.preventDefault();

        try {
            setError(false);
            setLoading(true);
            const token = await LoginUser({ email: username, password: password }, (e: any) => {
            });
            if (!token) {
                setError(true);
                setPassword('');
            }
            tokenCallback({ token: token.jwtBearerToken });
            location.href = location.origin;
        } catch {
            setError(true);
            setPassword('');
        }
        setLoading(false);
    };

    return (
        <div className="text-center login-wrapper">
            <Card style={{ width: 50 + 'vw' }}>
                <CardHeader>Please Log In</CardHeader>
                <CardBody>
                    <form onSubmit={handleSubmit}>
                        <div className="form-group">
                            <label>Email address</label>
                            <input type="email" className="form-control" value={username} onChange={(e: any) => setUserName(e.target.value)} placeholder="Enter email" />
                        </div>
                        <div className="form-group">
                            <label>Password</label>
                            <input type="password" className="form-control" value={password} onChange={(e: any) => setPassword(e.target.value)} placeholder="Enter password" />
                        </div>
                        <button type="submit" disabled={loading} className="btn btn-warning btn-block">Submit</button>
                    </form>
                </CardBody>
                {
                    error ?
                        <div className={'danger text-danger'}>Aanmelden mislukt, paswoord of email niet gevonden</div>
                        : null
                }
            </Card>
        </div>
    );
}