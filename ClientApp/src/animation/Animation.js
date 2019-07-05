export default class Animation {

    constructor(props) {
        this.canvas = props.canvas;
        this.width = parseInt(props.width);
        this.height = parseInt(props.height);

        this.init = this.init.bind(this);
        this.draw = this.draw.bind(this);
        this.loop = this.loop.bind(this);
    }

    // 初始化
    init() {
        this.canvasContext = this.canvas.getContext("2d");
        if (!this.canvasContext) {
            throw "浏览器不支持Canvas,请使用其他浏览器试试!"
        }
        this.canvas.width = this.width;
        this.canvas.height = this.height;
    }

    // 画每一帧
    draw() {

    }

    // 循环
    loop() {
        // 清空画布内容
        this.canvasContext.clearRect(0, 0, this.width, this.height);
    }
    
}