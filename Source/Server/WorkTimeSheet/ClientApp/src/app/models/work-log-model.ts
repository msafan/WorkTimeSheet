export class WorkLogModel {
    public id: number;
    public userId: number;
    public projectId: number;
    public startDateTime: Date;
    public endDateTime: Date;
    public remarks: string;
    public timeInSeconds: number;
    public name: string;
    public projectName: string;
}