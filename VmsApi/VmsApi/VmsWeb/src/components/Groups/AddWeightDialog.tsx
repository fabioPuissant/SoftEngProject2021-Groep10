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
  weightAdd: (
    id: string,
    groupNumber: number,
    date: string,
    amount: number,
    totalWeight: number,
    averageWeight: number
  ) => void;
  userDetail: UserDetails;
  token: string;
}

interface State {
  groupNumber: number;
  date: string;
  amount: number;
  totalWeight: number;
}

class AddWeightDialog extends React.Component<Props, State> {
  private userDetail: any;
  refs: any;
  constructor(props: Props) {
    super(props);
    this.state = {
      groupNumber: 0,
      date: "",
      amount: 0,
      totalWeight: 0,
    };
  }

  handleSave = async () => {
    let date = new Date(this.state.date);
    this.props.onClose();
    await doJsonPostApiCall(
      "MeasurePoint/Points", this.props.userDetail.token,
      { GroupNumber: this.state.groupNumber, date: date.toISOString(), amount: this.state.amount, weight: this.state.totalWeight},
      (weightData: any) => {
        this.props.weightAdd(
          weightData.id,
          weightData.pigGroup.groupNumber,
          weightData.date,  //TODO check in groups if date is later than previous date. or change to recapture new group info.
          weightData.amount,
          weightData.weight,
          weightData.weight / weightData.amount
        );

        this.setState({
          groupNumber: 0,
          date: "",
          amount: 0,
          totalWeight: 0,
        });
      }
    );
  };

  handleClose = () => {
    this.props.onClose();
  };

  render() {
    const { classes } = this.props;

    return (
      <Dialog
        open={this.props.open}
        onClose={this.handleClose}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
      >
        <DialogTitle id="alert-dialog-title">{"Gewicht toevoegen"}</DialogTitle>
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
                  value={this.state.date}
                  onChange={(e) => {
                    this.setState({date: e.target.value});
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
                <InputLabel htmlFor="groupWeightInput">gewicht:</InputLabel>
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

export default withHooksComponent(withStyles(tableStyle)(AddWeightDialog));
