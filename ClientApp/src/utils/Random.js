
/**
 * 在给出的范围内获取随机数。向上取整
 * @param {number} size 随机数范围,从0开始
 */
export function randomCeil(size) {
    return Math.ceil(Math.random() * size);
}

/**
 * 在给出的范围获取随机数。向下取整
 * @param {number} size 随机数范围，从0开始
 */
export function randomFloor(size) {
    return Math.floor(Math.random() * size);
}

/**
 * 在给出的范围内获取随机数
 * @param {number} min 随机数范围的最小值
 * @param {number} max 随机数范围的最大值
 */
export function random(min, max) {
    if (min >= max) {
        throw "参数不正确";
    }

    let random = Math.random();
    if (random == 0) {
        return min;
    }

    let size = max - min;
    random = random * size;
    return min + random;
}