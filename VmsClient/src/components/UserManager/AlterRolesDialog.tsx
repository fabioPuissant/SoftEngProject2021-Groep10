/* eslint-disable */
import React, { useState } from 'react';
import 'perfect-scrollbar/css/perfect-scrollbar.css';
// @material-ui/core components
import withStyles from '@material-ui/core/styles/withStyles';

// core components
import tableStyle from '../../assets/jss/material-dashboard-react/components/tableStyle';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogContentText from '@material-ui/core/DialogContentText';
import DialogTitle from '@material-ui/core/DialogTitle';
import Button from '@material-ui/core/Button';
import FormControl from '@material-ui/core/FormControl';
import { Input, InputLabel, NativeSelect } from '@material-ui/core';
import GridContainer from '../Grid/GridContainer';
import GridItem from '../Grid/GridItem';
import { Delete } from '@material-ui/icons';

// core components
type Callback = () => void;

interface Props {
    classes: any;
    open: boolean;
    user: UserPassing;
    onClose: Callback;
    onSuccess: Callback;
}

const AlterRolesDialog = (props: Props) => {
    const [user, setUser] = useState(props.user);
    const [roles, setRoles] = useState(user.roles);
    const [rolesNormilized, setRolesNormalized] = useState([...remapRoles(roles)]);
    const [newRole, setNewRole] = useState('');
    const [normalized, setNormalized] = useState('');
    const token = user.token;
    const classes = props.classes;
    const onSuccess: Callback = props.onSuccess;
    const onClose: Callback = props.onClose;

    const saveChanges = (e: any) => {
        const uniques = [...remapRoles(rolesNormilized)];
        fetch(`https://localhost:44320/api/Accounts/Roles/${user.id}`, {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json',
                'Accept': 'application/json',
            },
            body: JSON.stringify({ userid: user.id, roles: uniques }),
        })
            .then(e => {
                onSuccess();
            });
    };

    function remapRoles(rawRoles: Array<string>): Array<string> {
        const mapper: Map<string, string> = new Map<string, string>();
        mapper.set('Werknemer', 'EMPLOYEE');
        mapper.set('Werknemer', 'EMPLOYEE');
        mapper.set('EMPLOYEE', 'EMPLOYEE');
        mapper.set('Weger', 'WEGER');
        mapper.set('WEGER', 'WEGER');
        mapper.set('Voedingsbeheerder', 'NUTRITION');
        mapper.set('NUTRITION', 'NUTRITION');
        mapper.set('Agendabeheerder', 'AGENDA');
        mapper.set('AGENDA', 'AGENDA');
        mapper.set('Administratief_medewerker', 'ACCOUNTANT');
        mapper.set('ACCOUNTANT', 'ACCOUNTANT');
        mapper.set('Administrator', 'ADMINISTRATOR');
        mapper.set('ADMINISTRATOR', 'ADMINISTRATOR');
        mapper.set('Manager', 'MANAGER');
        mapper.set('MANAGER', 'MANAGER');
        mapper.set('Chief_Executive_Officer', 'CEO');
        mapper.set('CEO', 'CEO');
        const remapped = [...rawRoles.map(s => {
            const found = mapper.get(s);
            if (found) {
                return found;
            }
            return '';
        }).filter(r => r !== '')];
        const d = new Set(...remapped);
        const uniqeArray = new Array<string>();
        d.forEach(entry => uniqeArray.push(entry));

        return remapped;
    }

    const addRole = (e: any) => {
        if (!roles.includes(newRole)) {
            setRoles([...roles, newRole]);
            setRolesNormalized([...rolesNormilized, normalized]);
        }
        setNewRole('');
        setNormalized('');
    };

    return (
        <Dialog
            maxWidth={'md'}
            open={props.open}
            onClose={onClose}
            aria-labelledby="alert-dialog-title"
            aria-describedby="alert-dialog-description"

        >
            <DialogTitle id="alert-dialog-title">{`Rollen aanpassen ${user.firstname} ${user.lastname}`}</DialogTitle>
            <DialogContent>

                <GridContainer>
                    <GridItem xs={12} sm={12} md={8}>
                        <p >Rollen:</p>
                        {roles.map((r, k) => (
                            <GridContainer key={k} >
                                <GridItem xs={12} sm={12} md={12} >
                                    <InputLabel className={props.classes.labelRoot}>
                                        {r}  <Button variant="flat" color="inherit" onClick={() => {
                                        if (roles.length > 1) {
                                            setRoles(roles.filter(s => s !== r));
                                            setRolesNormalized([...(rolesNormilized.filter(s => s !== remapRoles([r])[0]))]);
                                        } else {
                                            alert('Een werknemer moet minstens één rol hebben!');
                                        }
                                    }} > <Delete /></Button></InputLabel>
                                </GridItem>

                            </GridContainer>
                        ))}
                    </GridItem>
                    <GridItem xs={12} sm={12} md={8}>
                        <FormControl className={classes.formControl}>
                            <InputLabel htmlFor="uncontrolled-native">Nieuwe Rol</InputLabel>
                            <NativeSelect
                                defaultValue={''}
                                inputProps={{
                                    name: 'Role',
                                    id: 'uncontrolled-native',
                                }}
                                onChange={(e) => {
                                    if (!e.target.value) { return; }
                                    setNormalized(e.target.value);
                                    setNewRole(e.target.value.substring(0, 1).toUpperCase() + e.target.value.substring(1, e.target.value.length).toLowerCase());
                                }}
                            >
                                <option aria-label="None" value="" />
                                <option value={'ADMINISTRATOR'}>Administrator</option>
                                <option value={'ACCOUNTANT'}>Accountant</option>
                                <option value={'AGENDA'}>Agendabeheerder</option>
                                <option value={'MANAGER'}>Manager</option>
                                <option value={'WEGER'}>Weger</option>
                                <option value={'NUTRITION'}>Voedingsbeheerder</option>
                                <option value={'EMPLOYEE'}>Werknemer</option>
                            </NativeSelect>
                        </FormControl >
                        <Button disabled={!newRole} onClick={addRole}>Voeg toe</Button>
                    </GridItem>
                </GridContainer>
            </DialogContent>
            <DialogActions>
                <Button color="primary" onClick={onClose}>
                    Annuleren
                </Button>
                <Button color="primary" autoFocus={true} onClick={saveChanges}>
                    Opslaan
                </Button>
            </DialogActions>
        </Dialog >
    );
};

export default withStyles(tableStyle)(AlterRolesDialog);
