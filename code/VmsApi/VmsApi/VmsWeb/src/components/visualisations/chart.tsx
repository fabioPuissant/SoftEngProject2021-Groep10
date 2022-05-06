/* eslint-disable */
import React from 'react';
import 'perfect-scrollbar/css/perfect-scrollbar.css';
// @material-ui/core components
import withStyles from '@material-ui/core/styles/withStyles';
import tableStyle from '../../assets/jss/material-dashboard-react/components/tableStyle';
// core components
import Dialog from "@material-ui/core/Dialog";
import DialogActions from "@material-ui/core/DialogActions";
import DialogContent from "@material-ui/core/DialogContent";
import DialogContentText from "@material-ui/core/DialogContentText";
import DialogTitle from "@material-ui/core/DialogTitle";
import Button from "@material-ui/core/Button";
import FormControl from "@material-ui/core/FormControl";
import { doJsonPostApiCall } from "../../utils/apicommunications";
import { FormHelperText, Input, InputLabel, NativeSelect } from "@material-ui/core";
import Plot from 'react-plotly.js';

interface Props {
    data: {}[];
    name: string;
    xaxis: {title: string};
    yaxis: {title: string};
  }
  
  interface State {
    
  }
  
  class LineChart extends React.Component<Props, State> {
    refs: any;
    constructor(props: Props) {
      super(props);
      this.state = {
      }
    }

  
    render() {

      return (
        <Plot data={this.props.data} layout={ {width: 1450, height: 500, title: this.props.name, xaxis:this.props.xaxis, yaxis:this.props.yaxis} }></Plot>
      );
    }
  }
  
  export default LineChart;