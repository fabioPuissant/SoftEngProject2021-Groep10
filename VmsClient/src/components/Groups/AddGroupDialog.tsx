/* eslint-disable */
import React from "react";
import "perfect-scrollbar/css/perfect-scrollbar.css";
// @material-ui/core components
import withStyles from "@material-ui/core/styles/withStyles";
// core components
import tableStyle from "../../assets/jss/material-dashboard-react/components/tableStyle";
import { withHooksComponent } from '../../custom-hooks/wrapperHook';
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import Button from "@material-ui/core/Button";
import { doJsonPostApiCall } from "../../utils/apicommunications";
import {
  FormControl,
  Input,
  InputLabel,
} from "@material-ui/core";

interface Props {
  classes: any;
  open: boolean;
  onClose: () => void;
  groupAdd: (
    id: string,
    groupNumber: number,
    birthDate: string,
    amount: number,
    totalWeight: number,
    averageWeight: number,
    foodWeight: number,
  ) => void;
  userDetail: UserDetails;
  token: string;
}

interface State {
  groupNumber: number;
  birthDate: string;
  amount: number;
  totalWeight: number;
  foodWeight: number;
}

class AddGroupDialog extends React.Component<Props, State> {
  private userDetail: any;
  refs: any;
  constructor(props: Props) {
    super(props);
    this.state = {
      groupNumber: 0,
      birthDate: "",
      amount: 0,
      totalWeight: 0,
      foodWeight:0,
    };

  }

  handleSave = async () => {
    let date = new Date(this.state.birthDate);
    this.props.onClose();
    await doJsonPostApiCall(
      "piggroup/groups", this.props.userDetail.token,
      { GroupNumber: this.state.groupNumber, birthDate: date.toISOString() },
      (groupData: any) => {
        // {"GroupNumber": nummer, "date": "2021-05-21T00:00:00.000Z",}  => returned aangemaakte groep.

        doJsonPostApiCall(
          "measurepoint/points", this.props.userDetail.token,
          { GroupNumber: this.state.groupNumber, date: date.toISOString(), weight: this.state.totalWeight, amount: this.state.amount },
          (data: any) => {}
        );

        doJsonPostApiCall(
          "FoodPurchase/Purchases", this.props.userDetail.token,
          { GroupNumber: this.state.groupNumber, date: date.toISOString(), amount: this.state.foodWeight },
          (data: any) => {}
        );

        this.props.groupAdd(
              groupData.id,
              groupData.groupNumber,
              groupData.birthDate,
              this.state.amount,
              this.state.totalWeight,
              this.state.totalWeight / this.state.amount,
              this.state.foodWeight
            );
      }
    );
  };

  handleClose = () => {
    this.props.onClose();
  };

  render() {
    const { classes, ...rest } = this.props;

    return (
      <Dialog
        open={this.props.open}
        onClose={this.handleClose}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
      >
        <DialogTitle id="alert-dialog-title">{"Groep toevoegen"}</DialogTitle>
        <DialogContent>
          <DialogContentText id="alert-dialog-description">
            <FormControl className={classes.margin}>
              <InputLabel htmlFor="groupNumberInput">groep nummer:</InputLabel>
              <Input
                id="groupNumberInput"
                type="number"
                value={this.state.groupNumber}
                onChange={(e) => {
                  this.setState({ groupNumber: Number(e.target.value) });
                }}
              />
            </FormControl>
            <div className={classes.margin}>
              <FormControl className={classes.margin}>
                <Input
                  id="groupDateInput"
                  placeholder=""
                  type={"date"}
                  value={this.state.birthDate}
                  onChange={(e) => {
                    this.setState({ birthDate: e.target.value });
                  }}
                />
              </FormControl>
            </div>
            <div className={classes.margin}>
              <FormControl className={classes.margin}>
                <InputLabel htmlFor="groupAmountInput">
                  aantal biggen:
                </InputLabel>
                <Input
                  id="groupAmountInput"
                  type={"number"}
                  value={this.state.amount}
                  onChange={(e) => {
                    this.setState({ amount: Number(e.target.value) });
                  }}
                />
              </FormControl>
              <FormControl className={classes.margin}>
                <InputLabel htmlFor="groupWeightInput">groep gewicht:</InputLabel>
                <Input
                  id="groupWeightInput"
                  type={"number"}
                  value={this.state.totalWeight}
                  onChange={(e) => {
                    this.setState({ totalWeight: Number(e.target.value) });
                  }}
                />
              </FormControl>
            </div>
            <div className={classes.margin}>
              <FormControl className={classes.margin}>
                <InputLabel htmlFor="foodWeightInput">voeder gewicht:</InputLabel>
                <Input
                  id="foodWeightInput"
                  type={"number"}
                  value={this.state.foodWeight}
                  onChange={(e) => {
                    this.setState({ foodWeight: Number(e.target.value) });
                  }}
                />
              </FormControl>
            </div>
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button color="primary" onClick={this.handleClose}>
            Annuleren
          </Button>
          <Button color="primary" autoFocus onClick={this.handleSave}>
            Opslaan
          </Button>
        </DialogActions>
      </Dialog>
    );
  }
}

export default withHooksComponent(withStyles(tableStyle)(AddGroupDialog));
