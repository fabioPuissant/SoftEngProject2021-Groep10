import React from 'react';
// @material-ui/core components
import withStyles from '@material-ui/core/styles/withStyles';
import InputLabel from '@material-ui/core/InputLabel';
// core components
import GridItem from '../../components/Grid/GridItem';
import GridContainer from '../../components/Grid/GridContainer';
import CustomInput from '../../components/CustomInput/CustomInput';
import Button from '../../components/CustomButtons/Button';
import Card from '../../components/Card/Card';
import CardHeader from '../../components/Card/CardHeader';
import CardAvatar from '../../components/Card/CardAvatar';
import CardBody from '../../components/Card/CardBody';
import CardFooter from '../../components/Card/CardFooter';

import avatar from '../../assets/img/faces/marc.jpg';
import { createStyles, FormControl } from '@material-ui/core';
import { withHooksComponent } from '../../custom-hooks/wrapperHook';
import useToken from '../../custom-hooks/useToken';
import { useEffect } from 'react';
import { useState } from 'react';

const styles = createStyles({
  cardCategoryWhite: {
    color: 'rgba(255,255,255,.62)',
    margin: '0',
    fontSize: '14px',
    marginTop: '0',
    marginBottom: '0'
  },
  cardTitleWhite: {
    color: '#FFFFFF',
    marginTop: '0px',
    minHeight: 'auto',
    fontWeight: 300,
    fontFamily: '\'Roboto\', \'Helvetica\', \'Arial\', sans-serif',
    marginBottom: '3px',
    textDecoration: 'none'
  }
});

function UserProfile(props: any) {
  const { classes } = props;
  const { userDetail, setToken } = useToken();
  const [isEnabled, setEnabled] = useState(false);
  const [oldpasswd, setOldPasswd] = useState('');
  const [passwd1, setPasswd1] = useState('');
  const [passwd2, setPasswd2] = useState('');
  const [detail, setDetail] = useState(userDetail ? userDetail : { firstname: 'undefined', roles: [], lastname: 'undefined', username: 'undefined', userref: 'undefined', token: 'undefined' });
  const roles: Array<string> = userDetail ? (typeof (userDetail.roles) === 'string' ? [userDetail.roles] : [...userDetail.roles]) : [];

  // Similar to componentDidMount and componentDidUpdate:

  function getRoleComponents() {
    const roleComponents = [];
    for (let i = 1; i <= roles.length; i++) {
      let ids = 'roles-' + i;
      roleComponents.push(<div><br /><br /></div>);
      roleComponents.push(
        <GridItem xs={12} sm={8} md={8} key={ids} >
          <InputLabel key={ids + "b"} className={classes.labelRoot} >
            {roles[i - 1]}
          </InputLabel>
        </GridItem>
      );
    }
    return roleComponents;
  }
  const userNumber = 'Werknemer nummer: ' + detail.userref.substr(0, 8);

  const empNum = () => {
    return (
      <InputLabel className={classes.labelRoot}>
        {userNumber}
      </InputLabel>
    );
  };

  const switchTo = async () => {
    if (isEnabled) {
      if (passwd1 !== passwd2) {
        setPasswd1('');
        setPasswd2('');
        setOldPasswd('');
        alert('Paswoorden moeten overeenkomen of opengelaten worden');
        return;
      }
      setEnabled(false);
      const postObj = {
        email: detail.username,
        firstname: detail.firstname,
        lastname: detail.lastname
      };

      let tokenString = `Bearer ${detail.token}`;

      const data = await fetch(`https://localhost:44320/api/Accounts/Update/${detail.userref}`, {
        method: 'PUT',
        headers: {
          'Authorization': tokenString,
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(postObj)
      });
      try {
        const token = await data.json();
        setToken({ token: token.jwtBearerToken });
        alert('Gegevens opgeslagen');
      } catch (e) {
        setPasswd1('');
        setPasswd2('');
        setOldPasswd('');
        return;
      }

      if (passwd1.length > 5) {
        fetch(
          `https://localhost:44320/api/Accounts/${detail.userref}/ChangePassword`, {
          method: 'POST',
          headers: {
            'Authorization': `Bearer ${detail.token}`,
            'Content-Type': 'application/json',
            'Accept': 'application/json',
          },
          body: JSON.stringify({ oldpassword: oldpasswd, password: passwd1, confirmpassword: passwd2 })
        }).then(e => {
          alert('Paswoord veranderd');

        })
          .catch(e => console.log('Failed to change password'))
          .finally(
            () => {
              setPasswd1('');
              setPasswd2('');
              setOldPasswd('');
            }
          );

      } else if (passwd1.length === 0 && passwd1 === '') {
        setPasswd1('');
        setPasswd2('');
        setOldPasswd('');
        return;
      } else {
        alert('Een paswoord moeten minstens 5 karakters bevatten!');
        setPasswd1('');
        setPasswd2('');
        // setOldPasswd('');
      }
    } else {
      setEnabled(true);
    }
  };

  return (
    <div>
      <GridContainer>
        <GridItem xs={12} sm={12} md={8}>
          <Card>
            <CardHeader color="primary">
              <h4 className={classes.cardTitleWhite}>Profiel</h4>
              <p className={classes.cardCategoryWhite}>Complete your profile</p>
            </CardHeader>
            <CardBody>
              <GridContainer>
                <GridItem xs={12} sm={12} md={12}>
                  {empNum()}
                </GridItem>
              </GridContainer>
              <GridContainer>
                <GridItem xs={12} sm={12} md={12}>
                  <CustomInput
                    labelText="Email adres"
                    id="email-address"
                    formControlProps={{
                      fullWidth: true
                    }}
                    enabled={false}
                    inputVal={detail.username}
                    changeCallback={(e: any) => {
                      setDetail({
                        firstname: detail.firstname,
                        lastname: detail.lastname,
                        roles: detail.roles,
                        userref: detail.userref,
                        token: detail.token,
                        username: e
                      });
                    }}
                  />
                </GridItem>
              </GridContainer>
              <GridContainer>
                <GridItem xs={12} sm={12} md={5}>
                  <CustomInput
                    labelText="Voornaam"
                    id="first-name"
                    formControlProps={{
                      fullWidth: true
                    }}
                    enabled={isEnabled}
                    inputVal={detail.firstname}

                    changeCallback={(e: any) => {
                      setDetail({
                        firstname: e,
                        lastname: detail.lastname,
                        roles: detail.roles,
                        userref: detail.userref,
                        token: detail.token,
                        username: detail.username
                      });
                    }}
                  />
                </GridItem>
                <GridItem xs={12} sm={12} md={5}>
                  <CustomInput
                    labelText="Familie naam"
                    id="last-name"
                    formControlProps={{
                      fullWidth: true
                    }}
                    enabled={isEnabled}
                    inputVal={detail.lastname}
                    changeCallback={(e: any) => {
                      setDetail({
                        firstname: detail.firstname,
                        lastname: e,
                        roles: detail.roles,
                        userref: detail.userref,
                        token: detail.token,
                        username: detail.username
                      });
                    }}
                  />
                </GridItem>
              </GridContainer>
              <GridContainer>
                <GridItem xs={12} sm={12} md={5}>
                  <CustomInput
                    labelText="Huidig Paswoord"
                    id="passwd-old"
                    formControlProps={{
                      fullWidth: true
                    }}
                    passwd={true}
                    enabled={isEnabled}
                    inputVal={oldpasswd}
                    changeCallback={(e: any) => {
                      setOldPasswd(e);
                    }}
                  />
                </GridItem>
                <GridItem xs={12} sm={12} md={5}>
                </GridItem>
                <GridItem xs={12} sm={12} md={5}>
                  <CustomInput
                    labelText="Nieuw Paswoord"
                    id="passwd-1"
                    formControlProps={{
                      fullWidth: true
                    }}
                    passwd={true}
                    enabled={isEnabled}
                    inputVal={passwd1}
                    changeCallback={(e: any) => {
                      setPasswd1(e);
                    }}
                  />
                </GridItem>
                <GridItem xs={12} sm={12} md={5}>
                  <CustomInput
                    labelText="Herhaal Nieuw Password"
                    id="passwd-2"
                    formControlProps={{
                      fullWidth: true
                    }}
                    passwd={true}
                    enabled={isEnabled}
                    inputVal={passwd2}
                    changeCallback={(e: any) => {
                      setPasswd2(e);
                    }}
                  />
                </GridItem>
              </GridContainer>
              <GridContainer>
                <GridItem xs={12} sm={12} md={12}>
                  <FormControl className={'fullWidth ' + classes.formControl}>
                    <InputLabel className={classes.labelRoot}>
                      Roles:
                    </InputLabel>
                  </FormControl>
                  {
                    getRoleComponents()
                  }
                </GridItem>
              </GridContainer>
            </CardBody>
            <CardFooter>
              <Button onClick={switchTo} color="primary">{isEnabled ? 'Sla op' : 'Update Profiel'} </Button>
            </CardFooter>
          </Card>
        </GridItem>
      </GridContainer>
    </div>
  );
}

export default withStyles(styles)(UserProfile);
