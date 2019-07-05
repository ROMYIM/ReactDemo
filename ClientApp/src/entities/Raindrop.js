import { random, randomFloor } from "../utils/Random";

export default class Raindrop {

    constructor(props) {

        // 传入画布的大小
        this.canvasWidth = props.canvasWidth;
        this.canvasHeight = props.canvasHeight;

        // 雨滴颜色
        this.color = props.color;

        // 雨滴透明度
        this.transparency = 0.8;

        // 雨滴的位置
        this.positionX = randomFloor(this.canvasWidth);
        this.positionY = random(this.canvasHeight * 0.8, this.canvasHeight * 0.9);

        // 雨滴下落的高度
        this.height = this.positionY;

        // 雨滴下落方向
        this.direction = 0;

        // 雨滴下落速度
        this.speed = random(8, 9);

    }
    
}